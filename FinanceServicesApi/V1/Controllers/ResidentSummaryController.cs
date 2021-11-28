using System.Threading.Tasks;
using FinanceServicesApi.V1.Boundary.Request;
using FinanceServicesApi.V1.Boundary.Responses;
using FinanceServicesApi.V1.Domain.Charges;
using FinanceServicesApi.V1.Factories;
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
        private readonly IGetAccountByIdUseCase _accountByIdUseCase;
        private readonly IGetTransactionByTargetIdUseCase _getTransactionByTargetIdUseCase;

        public ResidentSummaryController(IGetPersonByIdUseCase personUseCase
            ,IGetFinancialSummaryByTargetIdUseCase financialSummaryUseCase
            ,IGetChargeByTargetIdUseCase chargeUseCase
            ,IGetTenureInformationByIdUseCase tenureUseCase
            ,IGetContactDetailsByTargetIdUseCase contactUseCase
            ,IGetAccountByIdUseCase accountByIdUseCase
            ,IGetTransactionByTargetIdUseCase getTransactionByTargetIdUseCase)
        {
            _personUseCase = personUseCase;
            _financialSummaryUseCase = financialSummaryUseCase;
            _chargeUseCase = chargeUseCase;
            _tenureUseCase = tenureUseCase;
            _contactUseCase = contactUseCase;
            _accountByIdUseCase = accountByIdUseCase;
            _getTransactionByTargetIdUseCase = getTransactionByTargetIdUseCase;
        }

        /// <summary>
        /// Retrieving the all information relevant to the resident screen such as finance, tenure and contact information
        /// </summary>
        /// <param name="request">Consist of master account id and person id</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ConfirmTransferResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromQuery] ResidentSummaryRequest request)
        {
            var accountResponse = await _accountByIdUseCase.ExecuteAsync(request.MasterAccountId).ConfigureAwait(false);
            var personResponse =await _personUseCase.ExecuteAsync(request.PersonId).ConfigureAwait(false);
            var tenureResponse = await _tenureUseCase.ExecuteAsync(accountResponse.TargetId).ConfigureAwait(false);
            var transactionResponse = await _getTransactionByTargetIdUseCase.ExecuteAsync(accountResponse.TargetId)
                .ConfigureAwait(false);
            var chargeResponse = await _chargeUseCase.ExecuteAsync(tenureResponse.TenuredAsset.Id,TargetType.Asset).ConfigureAwait(false);
            var contactResponse = await _contactUseCase.ExecuteAsync(request.PersonId).ConfigureAwait(false);
            var summariesResponse = await _financialSummaryUseCase.ExecuteAsync(tenureResponse.TenuredAsset.Id, null, null).ConfigureAwait(false);

            return Ok(ResponseFactory.ToResponse(personResponse, tenureResponse, accountResponse, chargeResponse, contactResponse,
                summariesResponse, transactionResponse));
        }
    }
}
