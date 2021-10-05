using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using BaseApi.V1.Domain.SuspenseTransaction;
using BaseApi.V1.Infrastructure;
using BaseApi.V1.UseCase.Interfaces.SuspenseTransaction;
using Microsoft.AspNetCore.Http;

namespace BaseApi.V1.Controllers
{
    [ApiController]
    [Route("api/v1/info/suspenseAccount")]
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

        [ProducesResponseType(typeof(ConfirmTransferEntity), StatusCodes.Status200OK)]
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
            TryValidateModel(accountResponse);
            if (ModelState.IsValid)
            {
                if (accountResponse == null)
                {
                    return NotFound(new BaseErrorResponse((int) StatusCodes.Status404NotFound,
                        "No information by provided account id or account id founded!"));
                }
            }
            else
            {
                return BadRequest(new BaseErrorResponse((int) StatusCodes.Status400BadRequest,
                    ModelState.GetErrorMessages()));
            }

            var transactionResponse = await _getTransactionByIdUseCase.ExecuteAsync(transactionId).ConfigureAwait(false);
            TryValidateModel(transactionResponse);
            if (ModelState.IsValid)
            {
                if (transactionResponse == null)
                {
                    return NotFound(new BaseErrorResponse((int) StatusCodes.Status404NotFound,
                        "No information by provided transaction id or account id founded!"));
                }
            }
            else
            {
                return BadRequest(new BaseErrorResponse((int) StatusCodes.Status400BadRequest,
                    ModelState.GetErrorMessages()));
            }

            ConfirmTransferEntity result = Factories.EntityFactory.ToDomain(accountResponse, transactionResponse);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                /*This should not happen in any way*/
                throw new Exception("Something wrong happened in getting information!");
            }
        }
    }
}
