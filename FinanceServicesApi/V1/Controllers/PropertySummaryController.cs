using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Boundary.Request;
using FinanceServicesApi.V1.Boundary.Responses.ResidentSummary;
using FinanceServicesApi.V1.Factories;
using Microsoft.AspNetCore.Mvc;
using FinanceServicesApi.V1.Infrastructure;
using FinanceServicesApi.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Http;

namespace FinanceServicesApi.V1.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/v1/property-summary")]
    [ApiVersion("1.0")]
    public class PropertySummaryController : BaseController
    {
        private readonly IGetPersonByIdUseCase _personUseCase;
        private readonly IGetFinancialSummaryByTargetIdUseCase _financialSummaryUseCase;
        private readonly IGetChargeByAssetIdUseCase _chargeUseCase;
        private readonly IGetTenureInformationByIdUseCase _tenureUseCase;
        private readonly IGetContactDetailsByTargetIdUseCase _contactUseCase;
        private readonly IGetAccountByIdUseCase _accountUseCase;
        private readonly IGetLastPaymentTransactionsByTargetIdUseCase _transactionUseCase;
        private readonly IGetAssetByIdUseCase _assetUseCase;

        public PropertySummaryController(IGetPersonByIdUseCase personUseCase
            , IGetFinancialSummaryByTargetIdUseCase financialSummaryUseCase
            , IGetChargeByAssetIdUseCase chargeUseCase
            , IGetTenureInformationByIdUseCase tenureUseCase
            , IGetContactDetailsByTargetIdUseCase contactUseCase
            , IGetAccountByIdUseCase accountByIdUseCase
            , IGetLastPaymentTransactionsByTargetIdUseCase lastPaymentTransactionsByTargetIdUseCase
            , IGetAssetByIdUseCase assetByIdUseCase)
        {
            _personUseCase = personUseCase;
            _financialSummaryUseCase = financialSummaryUseCase;
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
        /// <param name="request">Master Account Id and person id who is the owner of the account</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResidentSummaryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] PropertySummaryRequest request)
        {
            var accountResponse =
                await _accountUseCase.ExecuteAsync(request.MasterAccountId).ConfigureAwait(false);

            var transactionResponse = (accountResponse == null || accountResponse.TargetId == Guid.Empty) ? null :
                await _transactionUseCase.ExecuteAsync(accountResponse.TargetId).ConfigureAwait(false);

            var tenureInformationResponse = (accountResponse == null || accountResponse.TargetId == Guid.Empty) ? null :
                await _tenureUseCase.ExecuteAsync(accountResponse.TargetId/*Guid.Parse("7495acd4-29af-49ad-8984-f30766f507af")*/).ConfigureAwait(false);

            var chargeResponse = (tenureInformationResponse == null ||
                                 tenureInformationResponse.TenuredAsset == null ||
                                 tenureInformationResponse.TenuredAsset.Id == Guid.Empty) ? null :
                await _chargeUseCase.ExecuteAsync(tenureInformationResponse.TenuredAsset.Id).ConfigureAwait(false);

            var contactDetailsResponse = (accountResponse == null ||
                                          accountResponse.Tenure==null ||
                                          accountResponse.Tenure.PrimaryTenants.ToList().Count==0) ?null:
                await _contactUseCase.ExecuteAsync(accountResponse.
                    Tenure.
                    PrimaryTenants.
                    First().Id).ConfigureAwait(false);

            var summaryResponse = (tenureInformationResponse == null ||
                                   tenureInformationResponse.TenuredAsset == null ||
                                   tenureInformationResponse.TenuredAsset.Id == Guid.Empty) ? null :
                await _financialSummaryUseCase.ExecuteAsync(tenureInformationResponse.TenuredAsset.Id, DateTime.UtcNow.AddDays(-7), DateTime.UtcNow).ConfigureAwait(false);

            var result = ResponseFactory.ToResponse(personResponse,
                tenureInformationResponse,
                accountResponse,
                chargeResponse,
                contactDetailsResponse,
                summaryResponse,
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
        [HttpGet("tenants/{id}")]
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
