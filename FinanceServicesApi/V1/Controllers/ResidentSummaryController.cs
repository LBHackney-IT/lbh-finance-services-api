using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Domain.SuspenseTransaction;
using FinanceServicesApi.V1.Gateways;
using FinanceServicesApi.V1.Infrastructure;
using FinanceServicesApi.V1.UseCase;
using Microsoft.AspNetCore.Http;

namespace FinanceServicesApi.V1.Controllers
{
    [ApiController]
    [Route("api/v1/resident-summary")]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    public class ResidentSummaryController : BaseController
    {

        /*public ResidentSummaryController()
        {

        }
        [ProducesResponseType(typeof(ConfirmTransferEntity), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            
        }*/
    }
}
