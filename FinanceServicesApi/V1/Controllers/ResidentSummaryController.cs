using System;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Boundary.Response;
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
        private readonly IGetTransactionByIdUseCase _transactionByIdUseCase;

        public ResidentSummaryController(IGetPersonByIdUseCase personUseCase
            ,IGetFinancialSummaryByTargetIdUseCase financialSummaryUseCase
            ,IGetChargeByTargetIdUseCase chargeUseCase
            ,IGetTenureInformationByIdUseCase tenureUseCase
            ,IGetContactDetailsByTargetIdUseCase contactUseCase
            ,IGetAccountByIdUseCase accountByIdUseCase
            ,IGetTransactionByIdUseCase transactionByIdUseCase)
        {
            _personUseCase = personUseCase;
            _financialSummaryUseCase = financialSummaryUseCase;
            _chargeUseCase = chargeUseCase;
            _tenureUseCase = tenureUseCase;
            _contactUseCase = contactUseCase;
            _accountByIdUseCase = accountByIdUseCase;
            _transactionByIdUseCase = transactionByIdUseCase;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Master Account Id</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ConfirmTransferResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var accountResponse = await _accountByIdUseCase.ExecuteAsync(id).ConfigureAwait(false);
            var transactionResponse = await _transactionByIdUseCase.ExecuteAsync()
            var tenureInformationResponse = await _tenureUseCase.ExecuteAsync(accountResponse.TargetId).ConfigureAwait(false)
            var personResponse =await _personUseCase.ExecuteAsync(id).ConfigureAwait(false);
            ResponseFactory.ToResponse(accountResponse,)
            /*var tenureResponse = await _tenureUseCase.ExecuteAsync()*/
        }
    }
}
