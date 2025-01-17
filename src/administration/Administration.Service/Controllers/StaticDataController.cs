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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.Eclipse.TractusX.Portal.Backend.Administration.Service.BusinessLogic;
using Org.Eclipse.TractusX.Portal.Backend.PortalBackend.DBAccess.Models;

namespace Org.Eclipse.TractusX.Portal.Backend.Administration.Service.Controllers;

/// <summary>
/// Controller providing actions for displaying, filtering and updating static data for app.
/// </summary>
[ApiController]
[Route("api/administration/staticdata")]
[Produces("application/json")]
[Consumes("application/json")]
public class StaticDataController : ControllerBase
{
    private readonly IStaticDataBusinessLogic _logic;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="logic">Business Logic</param>
    public StaticDataController(IStaticDataBusinessLogic logic)
    {
        _logic = logic;
    }

    /// <summary>
    /// Retrieves all Use Case Data
    /// </summary>
    /// <returns>AsyncEnumerable of Use Case Data</returns>
    /// <remarks>
    /// Example: GET: /api/administration/staticdata/usecases
    /// </remarks>
    /// <response code="200">Returns a list of all of the use case data.</response>
    [HttpGet]
    [Authorize(Roles = "view_use_cases")]
    [Route("usecases")]
    [ProducesResponseType(typeof(IAsyncEnumerable<UseCaseData>), StatusCodes.Status200OK)]
    public IAsyncEnumerable<UseCaseData> GetUseCases() =>
        _logic.GetAllUseCase();

    /// <summary>
    /// Retrieve all app language tags - short name (2digit) and long name
    /// </summary>
    /// <returns>AsyncEnumerable of Language Data</returns>
    /// <remarks>
    /// Example: GET: /api/administration/staticdata/languagetags
    /// the "lang" parameter is an optional parameter and if not set "en" will be used
    /// </remarks>
    /// <response code="200">Returns a list of all of the Language i.e german and english</response>
    [HttpGet]
    [Authorize(Roles = "view_app_language")]
    [Route("languagetags")]
    [ProducesResponseType(typeof(IAsyncEnumerable<LanguageData>), StatusCodes.Status200OK)]
    public IAsyncEnumerable<LanguageData> GetLanguages() =>
        _logic.GetAllLanguage();

    /// <summary>
    /// Retrieve all license types
    /// </summary>
    /// <returns>AsyncEnumerable of license type Data</returns>
    /// <remarks>
    /// Example: GET: /api/administration/staticdata/licenseType
    /// </remarks>
    /// <response code="200">Returns a list of all the license type i.e COTS and FOSS</response>
    [HttpGet]
    [Authorize(Roles = "view_license_types")]
    [Route("licenseType")]
    [ProducesResponseType(typeof(IAsyncEnumerable<LicenseTypeData>), StatusCodes.Status200OK)]
    public IAsyncEnumerable<LicenseTypeData> GetLicenseTypes() =>
        _logic.GetAllLicenseType();

    /// <summary>
    /// Retrieve the bpns of the operator companies
    /// </summary>
    /// <returns>AsyncEnumerable of the operator bpns</returns>
    /// <remarks>
    /// Example: GET: /api/administration/staticdata/operator-bpn
    /// </remarks>
    /// <response code="200">Returns a list of all the operator bpns</response>
    [HttpGet]
    [Authorize]
    [Route("operator-bpn")]
    [ProducesResponseType(typeof(IAsyncEnumerable<OperatorBpnData>), StatusCodes.Status200OK)]
    public IAsyncEnumerable<OperatorBpnData> GetOperatorBpns() =>
        _logic.GetOperatorBpns();
}
