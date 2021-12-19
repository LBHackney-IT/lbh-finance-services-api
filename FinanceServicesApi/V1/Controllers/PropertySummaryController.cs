using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon.Runtime.Internal;
using FinanceServicesApi.V1.Boundary.Request;
using FinanceServicesApi.V1.Boundary.Responses.PropertySummary;
using FinanceServicesApi.V1.Boundary.Responses.ResidentSummary;
using FinanceServicesApi.V1.Domain.ContactDetails;
using FinanceServicesApi.V1.Factories;
using Microsoft.AspNetCore.Mvc;
using FinanceServicesApi.V1.Infrastructure;
using FinanceServicesApi.V1.UseCase.Interfaces;
using Hackney.Shared.Tenure.Domain;
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
        private readonly IGetChargeByAssetIdUseCase _chargeUseCase;
        private readonly IGetTenureInformationByIdUseCase _tenureUseCase;
        private readonly IGetContactDetailsByTargetIdUseCase _contactUseCase;
        private readonly IGetAccountByIdUseCase _accountUseCase;
        private readonly IGetLastPaymentTransactionsByTargetIdUseCase _transactionUseCase;
        private readonly IGetAssetByIdUseCase _assetUseCase;

        public PropertySummaryController(IGetPersonByIdUseCase personUseCase
            , IGetChargeByAssetIdUseCase chargeUseCase
            , IGetTenureInformationByIdUseCase tenureUseCase
            , IGetContactDetailsByTargetIdUseCase contactUseCase
            , IGetAccountByIdUseCase accountByIdUseCase
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
        /// <param name="request">Master Account Id and person id who is the owner of the account</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ResidentSummaryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] PropertySummaryRequest request)
        {
            /*var accountResponse =
                await _accountUseCase.ExecuteAsync(request.MasterAccountId).ConfigureAwait(false);*/

            var transactionResponse =
                await _transactionUseCase.ExecuteAsync(request.TenureId).ConfigureAwait(false);

            var tenureInformationResponse =
                await _tenureUseCase.ExecuteAsync(request.TenureId).ConfigureAwait(false);

            var assetResponse = (tenureInformationResponse == null ||
                                tenureInformationResponse?.TenuredAsset == null ||
                                tenureInformationResponse?.TenuredAsset?.Id == null) ? null :
                await _assetUseCase.ExecuteAsync(tenureInformationResponse.TenuredAsset.Id).ConfigureAwait(false);

            var chargeResponse = (tenureInformationResponse == null ||
                                 tenureInformationResponse.TenuredAsset == null ||
                                 tenureInformationResponse.TenuredAsset.Id == Guid.Empty) ? null :
                await _chargeUseCase.ExecuteAsync(tenureInformationResponse.TenuredAsset.Id).ConfigureAwait(false);

            var personResponse = await _personUseCase.ExecuteAsync(request.PersonId).ConfigureAwait(false);

            var contactDetailsResponse = await _contactUseCase.ExecuteAsync(request.PersonId).ConfigureAwait(false);

            var result = ResponseFactory.ToResponse(tenureInformationResponse,
                personResponse,
                chargeResponse,
                contactDetailsResponse?.Results,
                transactionResponse,
                assetResponse);
            return Ok(result);
        }

        /// <summary>
        ///     Get all tenants relevant to the provided asset id
        /// </summary>
        /// <param name="id">Person Id</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<PropertySummaryTenantsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
        [HttpGet("tenants/{id}")]
        public async Task<IActionResult> GetTenantsByAssetId(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new BaseErrorResponse((int) HttpStatusCode.BadRequest,
                    $"{nameof(id)} cannot be empty."));

            var personData = await _personUseCase.ExecuteAsync(id).ConfigureAwait(false);
            if (personData == null)
                return NotFound(id);
            List<PropertySummaryTenantsResponse> response = new List<PropertySummaryTenantsResponse>();
            foreach (var t in personData.Tenures)
            {
                var tenureData = await _tenureUseCase.ExecuteAsync(t.Id).ConfigureAwait(false);
                List<ContactDetail> accountContactDetails = new List<ContactDetail>();
                var targetId = tenureData?.HouseholdMembers?.First(p => p.IsResponsible)?.Id ?? Guid.Empty;
                if (targetId != Guid.Empty)
                {
                    var tmpData = await _contactUseCase.ExecuteAsync(targetId).ConfigureAwait(false);
                    if (tmpData?.Results != null)
                        accountContactDetails.AddRange(tmpData.Results);
                }

                response.Add(ResponseFactory.ToResponse(personData, tenureData, accountContactDetails));
            }
            return Ok(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Tenure id</param>
        /// <returns>PropertyDetails</returns>
        [ProducesResponseType(typeof(PropertyDetailsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetPropertyDetailsByAssetId(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new BaseErrorResponse((int) HttpStatusCode.BadRequest,
                    $"The {nameof(id).ToString()} cannot be empty."));

            var tenureInformation = await _tenureUseCase.ExecuteAsync(id).ConfigureAwait(false);

            if (tenureInformation == null || tenureInformation.TenuredAsset?.Id == null || tenureInformation.TenuredAsset.Id == Guid.Empty)
                return NotFound(new BaseErrorResponse((int) HttpStatusCode.NotFound, $"There is no data for provided tenure"));

            var assetData =
                await _assetUseCase.ExecuteAsync(tenureInformation.TenuredAsset.Id).ConfigureAwait(false);

            if (assetData == null)
                return NotFound(new BaseErrorResponse((int) HttpStatusCode.NotFound, $"There is no data for provided tenure"));

            var chargeData = await _chargeUseCase.ExecuteAsync(id).ConfigureAwait(false);

            return Ok(ResponseFactory.ToResponse(assetData, chargeData));
        }
    }
}
