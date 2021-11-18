using System;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Boundary.Response;
using Microsoft.AspNetCore.Mvc;
using FinanceServicesApi.V1.Infrastructure;
using FinanceServicesApi.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Http;

namespace FinanceServicesApi.V1.Controllers
{
    [ApiController]
    [Route("api/v1/resident-summary")]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    public class ResidentSummaryController : BaseController
    {
        private readonly IGetPersonByIdUseCase _personUseCase;
        private readonly IGetFinancialSummaryByTargetIdUseCase _financialSummaryUseCase;
        private readonly IGetChargeByTargetIdUseCase _chargeUseCase;
        private readonly IGetTenureInformationByIdUseCase _tenureUseCase;
        private readonly IGetContactDetailsByTargetIdUseCase _contactUseCase;

        public ResidentSummaryController(IGetPersonByIdUseCase personUseCase
            ,IGetFinancialSummaryByTargetIdUseCase financialSummaryUseCase
            ,IGetChargeByTargetIdUseCase chargeUseCase
            ,IGetTenureInformationByIdUseCase tenureUseCase
            ,IGetContactDetailsByTargetIdUseCase contactUseCase)
        {
            _personUseCase = personUseCase;
            _financialSummaryUseCase = financialSummaryUseCase;
            _chargeUseCase = chargeUseCase;
            _tenureUseCase = tenureUseCase;
            _contactUseCase = contactUseCase;
        }

        [ProducesResponseType(typeof(ConfirmTransferResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var personResponse =await _personUseCase.ExecuteAsync(id).ConfigureAwait(false);
            var tenureResponse = await _tenureUseCase.ExecuteAsync()
        }
    }
}
