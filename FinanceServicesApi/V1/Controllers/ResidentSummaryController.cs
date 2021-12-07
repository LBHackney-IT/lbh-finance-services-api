using System;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Boundary.Requests;
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
        private readonly IGetLastPaymentTransactionsByTargetIdUseCase _lastPaymentTransactionsByTargetIdUseCase;

        public ResidentSummaryController(IGetPersonByIdUseCase personUseCase
            , IGetFinancialSummaryByTargetIdUseCase financialSummaryUseCase
            , IGetChargeByTargetIdUseCase chargeUseCase
            , IGetTenureInformationByIdUseCase tenureUseCase
            , IGetContactDetailsByTargetIdUseCase contactUseCase
            , IGetAccountByIdUseCase accountByIdUseCase
            , IGetLastPaymentTransactionsByTargetIdUseCase lastPaymentTransactionsByTargetIdUseCase)
        {
            _personUseCase = personUseCase;
            _financialSummaryUseCase = financialSummaryUseCase;
            _chargeUseCase = chargeUseCase;
            _tenureUseCase = tenureUseCase;
            _contactUseCase = contactUseCase;
            _accountByIdUseCase = accountByIdUseCase;
            _lastPaymentTransactionsByTargetIdUseCase = lastPaymentTransactionsByTargetIdUseCase;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request">Master Account Id and person id who is the owner of the account</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ConfirmTransferResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] ResidentSummaryRequest request)
        {
            var accountResponse =
                await _accountByIdUseCase.ExecuteAsync(request.MasterAccountId).ConfigureAwait(false);
            var transactionResponse =
                await _lastPaymentTransactionsByTargetIdUseCase.ExecuteAsync(Guid.Parse("42373628-07e7-4f9d-a755-86968b268d54")/*accountResponse.TargetId*/).ConfigureAwait(false);
            var tenureInformationResponse =
                await _tenureUseCase.ExecuteAsync(accountResponse.TargetId).ConfigureAwait(false);
            var personResponse =
                await _personUseCase.ExecuteAsync(request.PersonId).ConfigureAwait(false);
            var chargeResponse =
                await _chargeUseCase.ExecuteAsync(accountResponse.TargetId, TargetType.Tenure).ConfigureAwait(false);
            var contactDetailsResponse =
                await _contactUseCase.ExecuteAsync(request.PersonId).ConfigureAwait(false);
            var summaryResponse =
                await _financialSummaryUseCase.ExecuteAsync(tenureInformationResponse.TenuredAsset.Id, null, null).ConfigureAwait(false);

            var result = ResponseFactory.ToResponse(personResponse,
                tenureInformationResponse,
                accountResponse,
                chargeResponse,
                contactDetailsResponse,
                summaryResponse,
                transactionResponse);
            return Ok(result);
        }
    }
}
