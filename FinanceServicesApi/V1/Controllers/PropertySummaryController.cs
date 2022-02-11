using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Boundary.Responses.PropertySummary;
using FinanceServicesApi.V1.Domain.Charges;
using FinanceServicesApi.V1.Domain.ContactDetails;
using FinanceServicesApi.V1.Factories;
using Microsoft.AspNetCore.Mvc;
using FinanceServicesApi.V1.Infrastructure;
using FinanceServicesApi.V1.UseCase.Interfaces;
using Hackney.Shared.Asset.Domain;
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
        private readonly IGetAccountByTargetIdUseCase _accountByTargetIdUseCase;
        private readonly IGetLastPaymentTransactionsByTargetIdUseCase _transactionUseCase;
        private readonly IGetAssetByIdUseCase _assetUseCase;

        public PropertySummaryController(IGetPersonByIdUseCase personUseCase
            , IGetChargeByAssetIdUseCase chargeUseCase
            , IGetTenureInformationByIdUseCase tenureUseCase
            , IGetContactDetailsByTargetIdUseCase contactUseCase
            , IGetAccountByTargetIdUseCase accountByTargetIdUseCase
            , IGetLastPaymentTransactionsByTargetIdUseCase lastPaymentTransactionsByTargetIdUseCase
            , IGetAssetByIdUseCase assetByIdUseCase)
        {
            _personUseCase = personUseCase;
            _chargeUseCase = chargeUseCase;
            _tenureUseCase = tenureUseCase;
            _contactUseCase = contactUseCase;
            _accountByTargetIdUseCase = accountByTargetIdUseCase;
            _transactionUseCase = lastPaymentTransactionsByTargetIdUseCase;
            _assetUseCase = assetByIdUseCase;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Tenure id</param>
        /// <returns>Property Summary</returns>
        [ProducesResponseType(typeof(PropertySummaryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new BaseErrorResponse((int) HttpStatusCode.BadRequest,
                    $"{nameof(id)} cannot be empty."));

            var accountResponseTask = _accountByTargetIdUseCase.ExecuteAsync(id);
            var transactionResponseTask = _transactionUseCase.ExecuteAsync(id);
            var tenureInformationResponseTask = _tenureUseCase.ExecuteAsync(id);

            List<Task> tasks = new List<Task>()
            {
                { accountResponseTask },
                { transactionResponseTask },
                { tenureInformationResponseTask }
            };

            await Task.WhenAll(tasks).ConfigureAwait(false);
            tasks.Clear();

            var accountResponse = accountResponseTask.Result;
            var transactionResponse = transactionResponseTask.Result;
            var tenureInformationResponse = tenureInformationResponseTask.Result;

            var personId = tenureInformationResponse?.HouseholdMembers
                .FirstOrDefault(p => p.PersonTenureType == PersonTenureType.Leaseholder ||
                                     p.PersonTenureType == PersonTenureType.Tenant)?.Id ?? Guid.Empty;

            if (personId == Guid.Empty)
                return NotFound(new BaseErrorResponse((int) HttpStatusCode.NotFound, $"There is no data for provided tenure"));

            Asset assetResponse = null;
            List<Charge> chargeResponse = null;
            Task<Asset> assetResponseTask = null;
            Task<List<Charge>> chargeResponseTask = null;

            if (tenureInformationResponse?.TenuredAsset?.Id != null && tenureInformationResponse.TenuredAsset.Id != Guid.Empty)
            {
                assetResponseTask = _assetUseCase.ExecuteAsync(tenureInformationResponse.TenuredAsset.Id);
                chargeResponseTask = _chargeUseCase.ExecuteAsync(tenureInformationResponse.TenuredAsset.Id);
                tasks.AddRange(new List<Task> { { assetResponseTask }, { chargeResponseTask } });
            }

            var personResponseTask = _personUseCase.ExecuteAsync(personId);
            var contactDetailsResponseTask = _contactUseCase.ExecuteAsync(personId);

            tasks.AddRange(new List<Task> { { personResponseTask }, { contactDetailsResponseTask } });

            await Task.WhenAll(tasks).ConfigureAwait(false);
            assetResponse = assetResponseTask?.Result;
            chargeResponse = chargeResponseTask?.Result;

            var personResponse = personResponseTask.Result;
            var contactDetailsResponse = contactDetailsResponseTask.Result;

            var result = ResponseFactory.ToResponse(tenureInformationResponse,
                personResponse,
                accountResponse,
                chargeResponse,
                contactDetailsResponse?.Results,
                transactionResponse,
                assetResponse);
            return Ok(result);
        }

        /// <summary>
        ///     Get all tenants relevant to the provided asset id
        /// </summary>
        /// <param name="id">Tenure Id</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(List<PropertySummaryTenantsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
        [HttpGet("tenants/{id}")]
        public async Task<IActionResult> GetTenantsByTenureId(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new BaseErrorResponse((int) HttpStatusCode.BadRequest,
                    $"{nameof(id)} cannot be empty."));

            var tenureInformation = await _tenureUseCase.ExecuteAsync(id).ConfigureAwait(false);

            var personId = tenureInformation?.HouseholdMembers?
                .FirstOrDefault(p => p.PersonTenureType == PersonTenureType.Leaseholder ||
                                     p.PersonTenureType == PersonTenureType.Tenant)?.Id ?? Guid.Empty;

            if (personId == Guid.Empty)
                return NotFound(new BaseErrorResponse((int) HttpStatusCode.NotFound, $"There is no data for provided person"));

            var personData = await _personUseCase.ExecuteAsync(personId).ConfigureAwait(false);

            if (personData == null)
                return NotFound(new BaseErrorResponse((int) HttpStatusCode.NotFound, $"There is no data for provided person"));

            List<PropertySummaryTenantsResponse> response = new List<PropertySummaryTenantsResponse>();
            foreach (var t in personData.Tenures)
            {
                var tenureData = await _tenureUseCase.ExecuteAsync(t.Id).ConfigureAwait(false);
                if (tenureData == null)
                    return NotFound(new BaseErrorResponse((int) HttpStatusCode.NotFound, $"There is no data for provided tenure {t.Id.ToString()}"));

                List<ContactDetail> accountContactDetails = new List<ContactDetail>();
                var targetId = tenureData?.HouseholdMembers?.FirstOrDefault(p => p.IsResponsible)?.Id ?? Guid.Empty;
                if (targetId != Guid.Empty)
                {
                    var tmpData = await _contactUseCase.ExecuteAsync(targetId).ConfigureAwait(false);
                    if (tmpData?.Results != null)
                        accountContactDetails.AddRange(tmpData.Results);
                }

                response.Add(ResponseFactory.ToResponse(personData, tenureData, accountContactDetails));
            }

            if (response.Count == 0)
                return NotFound(new BaseErrorResponse((int) HttpStatusCode.NotFound, $"There is no data for provided person"));

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
                    $"{nameof(id)} cannot be empty."));

            var tenureInformation = await _tenureUseCase.ExecuteAsync(id).ConfigureAwait(false);

            if (tenureInformation?.TenuredAsset?.Id == null || tenureInformation.TenuredAsset.Id == Guid.Empty)
                return NotFound(new BaseErrorResponse((int) HttpStatusCode.NotFound, $"There is no data for provided tenure"));

            var assetData =
                await _assetUseCase.ExecuteAsync(tenureInformation.TenuredAsset.Id).ConfigureAwait(false);

            if (assetData == null)
                return NotFound(new BaseErrorResponse((int) HttpStatusCode.NotFound, $"There is no data for provided tenure"));

            var chargeData = await _chargeUseCase.ExecuteAsync(assetData.Id).ConfigureAwait(false);

            return Ok(ResponseFactory.ToResponse(assetData, chargeData));
        }

        [HttpGet("charges/{id}")]
        public async Task<IActionResult> GetChargesSummaryByType(Guid assetId)
        {
            var charges = await _chargeUseCase.ExecuteAsync(assetId).ConfigureAwait(false);

            if (charges == null || charges.Count == 0)
            {
                return NotFound(new BaseErrorResponse((int) HttpStatusCode.NotFound, $"no charges by this asset id"));
            }



            return Ok(assetId);
        }
    }
}
