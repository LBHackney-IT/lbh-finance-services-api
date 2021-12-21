using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Boundary.Request;
using FinanceServicesApi.V1.Boundary.Responses.ResidentSummary;
using FinanceServicesApi.V1.Domain.AccountModels;
using FinanceServicesApi.V1.Factories;
using Microsoft.AspNetCore.Mvc;
using FinanceServicesApi.V1.Infrastructure;
using FinanceServicesApi.V1.Infrastructure.Enums;
using FinanceServicesApi.V1.UseCase.Interfaces;
using Hackney.Shared.Person.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;

namespace FinanceServicesApi.V1.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/v1/resident-summary")]
    [ApiVersion("1.0")]
    public class ResidentSummaryController : BaseController
    {
        private readonly IGetPersonByIdUseCase _personUseCase;
        private readonly IGetChargeByAssetIdUseCase _chargeUseCase;
        private readonly IGetTenureInformationByIdUseCase _tenureUseCase;
        private readonly IGetContactDetailsByTargetIdUseCase _contactUseCase;
        private readonly IGetAccountByTargetIdUseCase _accountUseCase;
        private readonly IGetLastPaymentTransactionsByTargetIdUseCase _transactionUseCase;
        private readonly IGetAssetByIdUseCase _assetUseCase;

        public ResidentSummaryController(IGetPersonByIdUseCase personUseCase
            , IGetChargeByAssetIdUseCase chargeUseCase
            , IGetTenureInformationByIdUseCase tenureUseCase
            , IGetContactDetailsByTargetIdUseCase contactUseCase
            , IGetAccountByTargetIdUseCase accountByIdUseCase
            , IGetLastPaymentTransactionsByTargetIdUseCase lastPaymentTransactionsByTargetIdUseCase
            , IGetAssetByIdUseCase assetByIdUseCase)
        {
            _personUseCase = personUseCase;
            _chargeUseCase = chargeUseCase;
            _tenureUseCase = tenureUseCase;
            _contactUseCase = contactUseCase;
            _accountUseCase = accountByIdUseCase;
            _transactionUseCase = lastPaymentTransactionsByTargetIdUseCase;
            _assetUseCase = assetByIdUseCase;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Person Id</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResidentSummaryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id/*, PersonType personType*/)
        {
            var personResponse =
                await _personUseCase.ExecuteAsync(id).ConfigureAwait(false);
            if (personResponse == null)
                return NotFound(new BaseErrorResponse((int) HttpStatusCode.NotFound, $"There is no data for provided Person"));

            /*switch (personType)
            {
                case PersonType.Tenant:
                    {
                        if (!personResponse.Tenures.ToList().Any(p => p.Type.ToLower().Contains("tenant")))
                            return BadRequest(new BaseErrorResponse((int) HttpStatusCode.BadRequest,
                                $"The person is not a tenants."));
                        break;
                    }
                case PersonType.Leaseholder:
                {
                    if (!personResponse.Tenures.ToList().Any(p => p.Type.ToLower().Contains("leasehold")))
                        return BadRequest(new BaseErrorResponse((int) HttpStatusCode.BadRequest,
                            $"The person is not a leaseholder."));
                    break;
                }
            }*/

            Guid tenureId = Guid.Empty;
            Account account = new Account();
            foreach (var tenure in personResponse.Tenures)
            {
                account = null;
                var innerAccount =
                    await _accountUseCase.ExecuteAsync(tenure.Id).ConfigureAwait(false);
                if (innerAccount != null && innerAccount.ParentAccountId == Guid.Empty && innerAccount.AccountType == AccountType.Master)
                {
                    tenureId = tenure.Id;
                    account = innerAccount;
                    break;
                }
            }

            var transactionResponse = tenureId == Guid.Empty ? null :
                await _transactionUseCase.ExecuteAsync(tenureId).ConfigureAwait(false);

            var tenureInformationResponse = tenureId == Guid.Empty ? null :
                await _tenureUseCase.ExecuteAsync(tenureId).ConfigureAwait(false);

            var chargeResponse = (tenureInformationResponse?.TenuredAsset?.Id == null ||
                                  tenureInformationResponse?.TenuredAsset?.Id == Guid.Empty) ? null :
                await _chargeUseCase.ExecuteAsync(tenureInformationResponse.TenuredAsset.Id).ConfigureAwait(false);

            var contactDetailsResponse =
                await _contactUseCase.ExecuteAsync(id).ConfigureAwait(false);

            var result = ResponseFactory.ToResponse(personResponse,
                tenureInformationResponse,
                account,
                chargeResponse,
                contactDetailsResponse?.Results,
                transactionResponse);
            return Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Person Id</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResidentAssetsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
        [HttpGet("assets/{id}")]
        public async Task<IActionResult> GetAssetsByPersonId(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new BaseErrorResponse((int) HttpStatusCode.BadRequest,
                    $"{nameof(id).ToString()} cannot be empty."));

            var personData = await _personUseCase.ExecuteAsync(id).ConfigureAwait(false);
            if (personData == null)
                return NotFound(id);
            List<ResidentAssetsResponse> responses = new List<ResidentAssetsResponse>();
            foreach (var t in personData.Tenures)
            {
                var assetData = await _assetUseCase.ExecuteAsync(Guid.Parse(t.AssetId)).ConfigureAwait(false);
                var tenureData = await _tenureUseCase.ExecuteAsync(t.Id).ConfigureAwait(false);
                responses.Add(ResponseFactory.ToResponse(assetData, tenureData));
            }
            return Ok(responses);
        }
    }
}
