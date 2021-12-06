using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
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
        private readonly IGetAccountByIdUseCase _getAccountByIdUseCase;
        private readonly IGetTransactionByIdUseCase _getTransactionByIdUseCase;


        public SuspenseAccountController(IGetAccountByIdUseCase getAccountByIdUseCase, IGetTransactionByIdUseCase getTransactionByIdUseCase)
        {
            _getAccountByIdUseCase = getAccountByIdUseCase;
            _getTransactionByIdUseCase = getTransactionByIdUseCase;
        }

        [ProducesResponseType(typeof(ConfirmTransferResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] Guid transactionId, [FromQuery] Guid accountId)
        {
            if (transactionId == Guid.Empty || accountId == Guid.Empty)
                throw new ArgumentNullException(
                    paramName: $"The {nameof(accountId).ToString()} or {nameof(transactionId).ToString()} shouldn't be empty!");

            var accountResponse = await _getAccountByIdUseCase.ExecuteAsync(accountId).ConfigureAwait(false);
            if (accountResponse == null)
            {
                return NotFound(new BaseErrorResponse((int) StatusCodes.Status404NotFound,
                    "No information by provided account id or account id founded!"));
            }
            else if (!ModelValidatorHelper.IsModelValid(accountResponse))
            {
                return BadRequest(new BaseErrorResponse((int) StatusCodes.Status400BadRequest,
                    ModelValidatorHelper.ErrorMessages));
            }

            var transactionResponse = await _getTransactionByIdUseCase.ExecuteAsync(transactionId).ConfigureAwait(false);
            if (transactionResponse == null)
            {
                return NotFound(new BaseErrorResponse((int) StatusCodes.Status404NotFound,
                    "No information by provided transaction id or account id founded!"));
            }
            else if (!ModelValidatorHelper.IsModelValid(transactionResponse))
            {
                return BadRequest(new BaseErrorResponse((int) StatusCodes.Status400BadRequest,
                    ModelValidatorHelper.ErrorMessages));
            }

            ConfirmTransferResponse result = Factories.ResponseFactory.ToResponse(accountResponse, transactionResponse);
            return Ok(result);
            /*if (result != null)
            {
            }
            else
            {
                *//*This should not happen in any way*//*
                throw new Exception("Something wrong happened in getting information!");
            }*/
        }
    }
}
