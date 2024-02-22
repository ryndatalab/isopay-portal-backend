using Account.Service.BusinessLogic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.Eclipse.TractusX.Portal.Backend.Framework.ErrorHandling.Library;

namespace Account.Service.Controllers
{

    [ApiController]
    [Route("api/Account/FundTransfer")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class FundTransferController : ControllerBase
    {
        private readonly ITransaction _logic;

        /// <summary>
        /// Creates a new instance of <see cref="CompanyDataController"/>
        /// </summary>
        /// <param name="logic">The company data business logic</param>
        public FundTransferController(ITransaction logic)
        {
            _logic = logic;
        }

        [HttpPost]
        [Route("transfer")]
        //[Authorize]
        //[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        //[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        public async Task transfer([FromBody] TransferDto transferDto, CancellationToken cancellationToken)
        {
              _logic.Transfer(transferDto);
        }

    }
}
