/********************************************************************************
 * Copyright (c) 2021, 2023 Contributors to the Eclipse Foundation
 *
 * See the NOTICE file(s) distributed with this work for additional
 * information regarding copyright ownership.
 *
 * This program and the accompanying materials are made available under the
 * terms of the Apache License, Version 2.0 which is available at
 * https://www.apache.org/licenses/LICENSE-2.0.
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations
 * under the License.
 *
 * SPDX-License-Identifier: Apache-2.0
 ********************************************************************************/

using Microsoft.Extensions.Options;
using Org.Eclipse.TractusX.Portal.Backend.Framework.ErrorHandling;
using Org.Eclipse.TractusX.Portal.Backend.Mailing.SendMail;
using Org.Eclipse.TractusX.Portal.Backend.PortalBackend.DBAccess;
using Org.Eclipse.TractusX.Portal.Backend.PortalBackend.DBAccess.Models;
using Org.Eclipse.TractusX.Portal.Backend.PortalBackend.DBAccess.Repositories;
using Org.Eclipse.TractusX.Portal.Backend.PortalBackend.PortalEntities.Enums;
using Org.Eclipse.TractusX.Portal.Backend.Processes.NetworkRegistration.Library.DependencyInjection;
using Org.Eclipse.TractusX.Portal.Backend.Processes.NetworkRegistration.Library.Models;
using Org.Eclipse.TractusX.Portal.Backend.Provisioning.Library;
using Org.Eclipse.TractusX.Portal.Backend.Provisioning.Library.Models;
using Org.Eclipse.TractusX.Portal.Backend.Provisioning.Library.Service;

namespace Org.Eclipse.TractusX.Portal.Backend.Processes.NetworkRegistration.Library;

public class NetworkRegistrationHandler : INetworkRegistrationHandler
{
    private readonly IPortalRepositories _portalRepositories;
    private readonly IUserProvisioningService _userProvisioningService;
    private readonly IProvisioningManager _provisioningManager;
    private readonly NetworkRegistrationProcessSettings _settings;
    private readonly IMailingService _mailingService;

    public NetworkRegistrationHandler(
        IPortalRepositories portalRepositories,
        IUserProvisioningService userProvisioningService,
        IProvisioningManager provisioningManager,
        IMailingService mailingService,
        IOptions<NetworkRegistrationProcessSettings> options)
    {
        _portalRepositories = portalRepositories;
        _userProvisioningService = userProvisioningService;
        _provisioningManager = provisioningManager;
        _mailingService = mailingService;

        _settings = options.Value;
    }

    public async Task<(IEnumerable<ProcessStepTypeId>? nextStepTypeIds, ProcessStepStatusId stepStatusId, bool modified, string? processMessage)> SynchronizeUser(Guid networkRegistrationId)
    {
        var userRepository = _portalRepositories.GetInstance<IUserRepository>();
        var userRoleRepository = _portalRepositories.GetInstance<IUserRolesRepository>();
        var ospName = await _portalRepositories.GetInstance<INetworkRepository>().GetOspCompanyName(networkRegistrationId).ConfigureAwait(false);
        if (string.IsNullOrWhiteSpace(ospName))
        {
            throw new UnexpectedConditionException("Onboarding Service Provider name must be set");
        }

        var companyAssignedIdentityProviders = await userRepository
            .GetUserAssignedIdentityProviderForNetworkRegistration(networkRegistrationId)
            .ToListAsync()
            .ConfigureAwait(false);
        var roleData = await _userProvisioningService.GetRoleDatas(_settings.InitialRoles).ToListAsync().ConfigureAwait(false);

        foreach (var cu in companyAssignedIdentityProviders)
        {
            if (string.IsNullOrWhiteSpace(cu.FirstName) || string.IsNullOrWhiteSpace(cu.LastName) ||
                string.IsNullOrWhiteSpace(cu.Email))
            {
                throw new ConflictException(
                    $"Firstname, Lastname & Email of CompanyUser {cu.CompanyUserId} must not be null here");
            }

            if (cu.ProviderLinkData.Any(x => string.IsNullOrWhiteSpace(x.Alias)))
            {
                throw new ConflictException($"Alias must be set for all ProviderLinkData of CompanyUser {cu.CompanyUserId}");
            }

            try
            {
                var userId = await _provisioningManager.GetUserByUserName(cu.CompanyUserId.ToString()).ConfigureAwait(false);
                if (!string.IsNullOrWhiteSpace(userId))
                {
                    userRepository.AttachAndModifyIdentity(cu.CompanyUserId, i =>
                        {
                            i.UserStatusId = UserStatusId.PENDING;
                        },
                        i =>
                        {
                            i.UserStatusId = UserStatusId.ACTIVE;
                        });

                    await _userProvisioningService.AssignRolesToNewUserAsync(userRoleRepository, roleData, (userId, cu.CompanyUserId)).ConfigureAwait(false);
                    continue;
                }

                await _userProvisioningService.HandleCentralKeycloakCreation(new UserCreationRoleDataIdpInfo(cu.FirstName!, cu.LastName!, cu.Email!, roleData, string.Empty, string.Empty, UserStatusId.ACTIVE, true), cu.CompanyUserId, cu.CompanyName, cu.Bpn, null, cu.ProviderLinkData.Select(x => new IdentityProviderLink(x.Alias!, x.ProviderUserId, x.UserName)), userRepository, userRoleRepository).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                throw new ServiceException(e.Message, true);
            }
        }

        await SendMails(GetUserMailInformation(companyAssignedIdentityProviders), ospName).ConfigureAwait(false);
        return new ValueTuple<IEnumerable<ProcessStepTypeId>?, ProcessStepStatusId, bool, string?>(
            null,
            ProcessStepStatusId.DONE,
            false,
            null);
    }

    private async IAsyncEnumerable<UserMailInformation> GetUserMailInformation(IEnumerable<CompanyUserIdentityProviderProcessData> companyUserIdentityProviderProcessData)
    {
        var mapping = new Dictionary<string, string>();

        async Task<string> GetDisplayName(string idpAlias)
        {
            if (!mapping.TryGetValue(idpAlias, out var displayName))
            {
                displayName = await _provisioningManager.GetIdentityProviderDisplayName(idpAlias).ConfigureAwait(false);
                if (string.IsNullOrWhiteSpace(displayName))
                {
                    throw new ConflictException($"DisplayName for idpAlias {idpAlias} couldn't be determined");
                }
                mapping.Add(idpAlias, displayName);
            }
            return displayName;
        }

        foreach (var userData in companyUserIdentityProviderProcessData)
        {
            yield return new UserMailInformation(
                userData.Email ?? throw new UnexpectedConditionException("userData.Email should never be null here"),
                userData.FirstName,
                userData.LastName,
                await Task.WhenAll(
                    userData.ProviderLinkData.Select(pld =>
                        GetDisplayName(pld.Alias ?? throw new UnexpectedConditionException("providerLinkData.Alias should never be null here")))).ConfigureAwait(false));
        }
    }

    private static readonly IEnumerable<string> MailTemplates = Enumerable.Repeat("OspWelcomeMail", 1);

    private async Task SendMails(IAsyncEnumerable<UserMailInformation> companyUserWithRoleIdForCompany, string ospName)
    {
        await foreach (var (receiver, firstName, lastName, displayNames) in companyUserWithRoleIdForCompany)
        {
            var userName = string.Join(" ", firstName, lastName);
            var mailParameters = new Dictionary<string, string>
            {
                { "userName", !string.IsNullOrWhiteSpace(userName) ? userName : receiver },
                { "osp", ospName },
                { "loginDocumentUrl", _settings.LoginDocumentAddress },
                { "externalRegistrationUrl", _settings.ExternalRegistrationAddress },
                { "closeApplicationUrl", _settings.CloseApplicationAddress },
                { "url", _settings.BasePortalAddress },
                { "idpAlias", string.Join(",", displayNames) }
            };
            await _mailingService.SendMails(receiver, mailParameters, MailTemplates).ConfigureAwait(false);
        }
    }
}
