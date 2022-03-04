using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Boundary.Responses;
using FinanceServicesApi.V1.Infrastructure;
using FinanceServicesApi.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Http;

namespace FinanceServicesApi.V1.Controllers
{
    [ApiController]
    [Route("api/v1/suspenseAccount")]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    public class SuspenseAccountController : BaseController
    {
        private readonly IGetTransactionByIdUseCase _getTransactionByIdUseCase;
        private readonly IGetAccountByTargetIdUseCase _getAccountByTargetIdUseCase;


        public SuspenseAccountController(IGetTransactionByIdUseCase getTransactionByIdUseCase, IGetAccountByTargetIdUseCase getAccountByTargetIdUseCase)
        {
            _getTransactionByIdUseCase = getTransactionByIdUseCase;
            _getAccountByTargetIdUseCase = getAccountByTargetIdUseCase;
        }

        [ProducesResponseType(typeof(ConfirmTransferResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] Guid transactionId, [FromQuery] Guid tenureId)
        {
            if (transactionId == Guid.Empty || tenureId == Guid.Empty)
                throw new ArgumentException($"The {nameof(tenureId)} or {nameof(transactionId)} shouldn't be empty!");

            var accountResponse = await _getAccountByTargetIdUseCase.ExecuteAsync(tenureId).ConfigureAwait(false);
            if (accountResponse == null)
            {
                return NotFound(new BaseErrorResponse(StatusCodes.Status404NotFound,
                    "No information by provided transaction id or tenure id founded!"));
            }
            else if (!ModelValidatorHelper.IsModelValid(accountResponse))
            {
                return BadRequest(new BaseErrorResponse(StatusCodes.Status400BadRequest,
                    ModelValidatorHelper.ErrorMessages));
            }

            var transactionResponse = await _getTransactionByIdUseCase.ExecuteAsync(transactionId).ConfigureAwait(false);
            if (transactionResponse == null)
            {
                return NotFound(new BaseErrorResponse(StatusCodes.Status404NotFound,
                    "No information by provided transaction id or tenure id founded!"));
            }
            else if (!ModelValidatorHelper.IsModelValid(transactionResponse))
            {
                return BadRequest(new BaseErrorResponse(StatusCodes.Status400BadRequest,
                    ModelValidatorHelper.ErrorMessages));
            }

            ConfirmTransferResponse result = Factories.ResponseFactory.ToResponse(accountResponse, transactionResponse);
            return Ok(result);
        }
    }
}
