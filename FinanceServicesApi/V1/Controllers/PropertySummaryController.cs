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
using FinanceServicesApi.V1.Infrastructure.Enums;
using FinanceServicesApi.V1.UseCase.Interfaces;
using Hackney.Shared.Asset.Domain;
using Hackney.Shared.Tenure.Domain;
using Microsoft.AspNetCore.Http;
using FinanceServicesApi.V1.Boundary.Request.Enums;
using FinanceServicesApi.V1.Boundary.Responses;

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
        private readonly IGetAssetApportionmentUseCase _getAssetApportionmentUseCase;

        public PropertySummaryController(IGetPersonByIdUseCase personUseCase
            , IGetChargeByAssetIdUseCase chargeUseCase
            , IGetTenureInformationByIdUseCase tenureUseCase
            , IGetContactDetailsByTargetIdUseCase contactUseCase
            , IGetAccountByTargetIdUseCase accountByTargetIdUseCase
            , IGetLastPaymentTransactionsByTargetIdUseCase lastPaymentTransactionsByTargetIdUseCase
            , IGetAssetByIdUseCase assetByIdUseCase
            , IGetAssetApportionmentUseCase getAssetApportionmentUseCase)
        {
            _personUseCase = personUseCase;
            _chargeUseCase = chargeUseCase;
            _tenureUseCase = tenureUseCase;
            _contactUseCase = contactUseCase;
            _accountByTargetIdUseCase = accountByTargetIdUseCase;
            _transactionUseCase = lastPaymentTransactionsByTargetIdUseCase;
            _assetUseCase = assetByIdUseCase;
            _getAssetApportionmentUseCase = getAssetApportionmentUseCase;
        }

        /// <summary>
        /// Return summary info about some property, including HousingBenefit, ServiceCharge, CurrentBalance, Rent, etc.
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
            TenureInformation tenureInformationResponse = tenureInformationResponseTask.Result;

            if (tenureInformationResponse == null)
                return NotFound($"The tenure id not found!");

            var personId = tenureInformationResponse?.HouseholdMembers.FirstOrDefault(p => p.IsResponsible)?.Id ?? Guid.Empty;

            if (personId == Guid.Empty)
                return NotFound(new BaseErrorResponse((int) HttpStatusCode.NotFound, $"There is no responsible household member for provided tenure."));

            Asset assetResponse = null;
            Charge chargeResponse = null;
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
            var rawChargeResponse = chargeResponseTask?.Result;


            // leaseholder, if the value is true
            var isLeaseHolder =
                        tenureInformationResponse.TenureType?.Description == TenureTypes.LeaseholdRTB.Description
                     || tenureInformationResponse.TenureType?.Description == TenureTypes.PrivateSaleLH.Description
                     || tenureInformationResponse.TenureType?.Description == TenureTypes.SharedOwners.Description
                     || tenureInformationResponse.TenureType?.Description == TenureTypes.SharedEquity.Description
                     || tenureInformationResponse.TenureType?.Description == TenureTypes.ShortLifeLse.Description
                     || tenureInformationResponse.TenureType?.Description == TenureTypes.LeaseholdStair.Description
                     || tenureInformationResponse.TenureType?.Description == TenureTypes.FreeholdServ.Description;

            if (isLeaseHolder) // leaseholder
            {
                var financialYear = DateTime.UtcNow.Year + ((DateTime.UtcNow.Month > 0 && DateTime.UtcNow.Month < 4) ? -1 : 0);

                chargeResponse = rawChargeResponse?.Where(p => p.ChargeGroup == ChargeGroup.Leaseholders
                                                               && p.ChargeSubGroup == ChargeSubGroup.Estimate
                                                               && p.ChargeYear == financialYear).FirstOrDefault();
            }
            else
            {
                chargeResponse = rawChargeResponse?.Where(p => p.ChargeGroup == ChargeGroup.Tenants)
                    .OrderByDescending(c => c.ChargeYear).FirstOrDefault();
            }

            var personResponse = personResponseTask.Result;

            var contactDetailsResponse = contactDetailsResponseTask.Result;

            var result = ResponseFactory.ToResponse(tenureInformationResponse,
                personResponse,
                accountResponse,
                chargeResponse,
                contactDetailsResponse?.Results,
                transactionResponse,
                assetResponse,
                isLeaseHolder);
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

            var personId = tenureInformation?.HouseholdMembers?.FirstOrDefault(p => p.IsResponsible)?.Id ?? Guid.Empty;

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

            var rawChargeData = await _chargeUseCase.ExecuteAsync(assetData.Id).ConfigureAwait(false);
            Charge chargeData = null;

            // leaseholder, if the value is true
            var isLeaseHolder =
                   tenureInformation.TenureType?.Description == TenureTypes.LeaseholdRTB.Description
                || tenureInformation.TenureType?.Description == TenureTypes.PrivateSaleLH.Description
                || tenureInformation.TenureType?.Description == TenureTypes.SharedOwners.Description
                || tenureInformation.TenureType?.Description == TenureTypes.SharedEquity.Description
                || tenureInformation.TenureType?.Description == TenureTypes.ShortLifeLse.Description
                || tenureInformation.TenureType?.Description == TenureTypes.LeaseholdStair.Description
                || tenureInformation.TenureType?.Description == TenureTypes.FreeholdServ.Description;

            if (isLeaseHolder) // leaseholder
            {
                var financialYear = DateTime.UtcNow.Year + ((DateTime.UtcNow.Month > 0 && DateTime.UtcNow.Month < 4) ? -1 : 0);

                chargeData = rawChargeData?.Where(p => p.ChargeGroup == ChargeGroup.Leaseholders
                                                       && p.ChargeSubGroup == ChargeSubGroup.Estimate
                                                       && p.ChargeYear == financialYear).FirstOrDefault();
            }
            else
            {
                chargeData = rawChargeData?.Where(p => p.ChargeGroup == ChargeGroup.Tenants)
                    .OrderByDescending(c => c.ChargeYear).FirstOrDefault();
            }

            return Ok(ResponseFactory.ToResponse(assetData, chargeData, isLeaseHolder));
        }

        /// <summary>
        /// Returns Totals, Estate costs, Block costs, Property costs
        /// </summary>
        /// <param name="assetId"></param>
        /// <param name="fromYear">Start year for totals sequense. Will be returned result for period from provided year until currect one. Should be more that 1970 ans less than currect year</param>
        /// <param name="chargeGroupFilter">Defines for what charge group we need to return response. Allowed values: 0 [Tenants], 1 [Leaseholders], 2 [Both]</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(AssetApportionmentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{assetId}/apportionments")]
        public async Task<IActionResult> GetAssetApportionments([FromRoute] Guid assetId,
            [FromQuery] short fromYear,
            [FromQuery] ChargeGroupFilter chargeGroupFilter = ChargeGroupFilter.Both)
        {
            if (assetId == Guid.Empty)
            {
                return BadRequest(new BaseErrorResponse((int) HttpStatusCode.BadRequest,
                    $"{nameof(assetId)} cannot be empty."));
            }
            if (fromYear < 1970 || fromYear > DateTime.UtcNow.Year)
            {
                return BadRequest(new BaseErrorResponse((int) HttpStatusCode.BadRequest,
                    $"{nameof(fromYear)} should be more that 1970 ans less than currect year"));
            }

            var result = await _getAssetApportionmentUseCase.ExecuteAsync(assetId, fromYear, chargeGroupFilter).ConfigureAwait(false);

            return Ok(result);
        }


        /// <summary>
        /// Gets yearly rent debits for an asset.
        /// </summary>
        /// <param name="assetId">Asset id.</param>
        /// <param name="useCase">Get debits use case.</param>
        /// <returns>List of yearly rent debits</returns>
        [ProducesResponseType(typeof(IEnumerable<YearlyRentDebitResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{assetId}/yearly-rent-debits")]
        public async Task<ActionResult<IEnumerable<YearlyRentDebitResponse>>> GetYearlyRentDebitsAsync([FromRoute] Guid assetId, [FromServices] IGetYearlyRentDebitsUseCase useCase)
        {
            var result = await useCase.ExecuteAsync(assetId).ConfigureAwait(false);

            return Ok(result);
        }
    }
}
