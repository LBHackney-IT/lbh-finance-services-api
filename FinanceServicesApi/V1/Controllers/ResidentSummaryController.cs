using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Boundary.Responses.ResidentSummary;
using FinanceServicesApi.V1.Domain.AccountModels;
using FinanceServicesApi.V1.Domain.Charges;
using FinanceServicesApi.V1.Domain.TransactionModels;
using FinanceServicesApi.V1.Factories;
using Microsoft.AspNetCore.Mvc;
using FinanceServicesApi.V1.Infrastructure;
using FinanceServicesApi.V1.Infrastructure.Enums;
using FinanceServicesApi.V1.UseCase.Interfaces;
using Hackney.Shared.Tenure.Domain;
using Microsoft.AspNetCore.Http;

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
        public async Task<IActionResult> GetById(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new BaseErrorResponse((int) HttpStatusCode.BadRequest,
                    $"{nameof(id)} cannot be empty."));

            List<Task> tasks = new List<Task>();

            var personResponseTask = _personUseCase.ExecuteAsync(id);
            var contactDetailsResponseTask = _contactUseCase.ExecuteAsync(id);

            tasks.AddRange(new List<Task>
            {
                {personResponseTask},
                {contactDetailsResponseTask}
            });

            await Task.WhenAll(tasks).ConfigureAwait(false);
            tasks.Clear();

            var personResponse = personResponseTask.Result;
            var contactDetailsResponse = contactDetailsResponseTask.Result;

            if (personResponse == null)
                return NotFound(new BaseErrorResponse((int) HttpStatusCode.NotFound, $"There is no data for provided Person"));

            Guid tenureId = Guid.Empty;
            Account account = new Account();
            foreach (var tenure in personResponse.Tenures.Where(p=>p.IsActive))
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

            Task<List<Transaction>> transactionResponseTask = null;
            Task<TenureInformation> tenureInformationResponseTask = null;
            if (tenureId != Guid.Empty)
            {
                transactionResponseTask = _transactionUseCase.ExecuteAsync(tenureId);
                tenureInformationResponseTask = _tenureUseCase.ExecuteAsync(tenureId);
                tasks.AddRange(new List<Task>{
                    {transactionResponseTask},{tenureInformationResponseTask}
                });
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);

            var transactionResponse = transactionResponseTask?.Result;
            var tenureInformationResponse = tenureInformationResponseTask?.Result;

            if (tenureInformationResponse == null)
                return NotFound(new BaseErrorResponse((int) HttpStatusCode.NotFound, $"There is no data for provided tenure"));

            var chargeResponse = (tenureInformationResponse?.TenuredAsset?.Id == null ||
                                  tenureInformationResponse?.TenuredAsset?.Id == Guid.Empty) ? null :
                await _chargeUseCase.ExecuteAsync(tenureInformationResponse.TenuredAsset.Id).ConfigureAwait(false);

            Charge chargeData = null;

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

                chargeData = chargeResponse?.Where(p => p.ChargeGroup == ChargeGroup.Leaseholders
                                                        && p.ChargeSubGroup == ChargeSubGroup.Estimate
                                                        && p.ChargeYear == financialYear).FirstOrDefault();
            }
            else
            {
                chargeData = chargeResponse?.Where(p => p.ChargeGroup == ChargeGroup.Tenants)
                    .OrderByDescending(c => c.ChargeYear).FirstOrDefault();
            }

            var result = ResponseFactory.ToResponse(personResponse,
                tenureInformationResponse,
                account,
                chargeData,
                contactDetailsResponse?.Results,
                transactionResponse,
                isLeaseHolder);
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
                    $"{nameof(id)} cannot be empty."));

            var personData = await _personUseCase.ExecuteAsync(id).ConfigureAwait(false);

            if (personData == null)
                return NotFound(new BaseErrorResponse((int) HttpStatusCode.NotFound, $"There is no data for provided tenure"));

            List<Task> tasks = new List<Task>();

            List<ResidentAssetsResponse> responses = new List<ResidentAssetsResponse>();
            foreach (var t in personData.Tenures)
            {
                tasks.Clear();
                var assetTask = _assetUseCase.ExecuteAsync(Guid.Parse(t.AssetId));
                var tenureTask = _tenureUseCase.ExecuteAsync(t.Id);
                var chargeTask = _chargeUseCase.ExecuteAsync(Guid.Parse(t.AssetId));
                var accountTask = _accountUseCase.ExecuteAsync(t.Id);

                tasks.AddRange(new List<Task> { assetTask, tenureTask, chargeTask });

                await Task.WhenAll(tasks).ConfigureAwait(false);

                var assetData = assetTask?.Result;
                var tenureData = tenureTask?.Result;
                var chargeData = chargeTask?.Result;
                var accountData = accountTask?.Result;

                responses.Add(ResponseFactory.ToResponse(assetData, tenureData, chargeData, accountData));
            }
            return Ok(responses);
        }
    }
}
