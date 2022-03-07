using FinanceServicesApi.V1.Boundary.Request;
using FinanceServicesApi.V1.Boundary.Responses;
using FinanceServicesApi.V1.Boundary.Responses.PropertySummary;
using FinanceServicesApi.V1.Domain.Charges;
using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.Infrastructure.Enums;
using FinanceServicesApi.V1.UseCase.Interfaces;
using Hackney.Shared.Asset.Domain;
using Hackney.Shared.Tenure.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace FinanceServicesApi.V1.UseCase
{
    public class GetLeaseholdAssetsListUseCase : IGetLeaseholdAssetsListUseCase
    {
        private readonly IHousingSearchGateway _housingSearchGateway;
        private readonly IGetChargeByAssetIdUseCase _getChargeByAssetIdUseCase;
        private readonly ILogger<GetLeaseholdAssetsListUseCase> _logger;

        public GetLeaseholdAssetsListUseCase(IHousingSearchGateway housingSearchGateway,
            IGetChargeByAssetIdUseCase getChargeByAssetIdUseCase,
            ILogger<GetLeaseholdAssetsListUseCase> logger)
        {
            _housingSearchGateway = housingSearchGateway;
            _getChargeByAssetIdUseCase = getChargeByAssetIdUseCase;
            _logger = logger;
        }

        public async Task<GetPropertyListResponse> ExecuteAsync(LeaseholdAssetsRequest housingSearchRequest)
        {
            var assetList = await _housingSearchGateway.GetAssets(housingSearchRequest.SearchText, housingSearchRequest.AssetType).ConfigureAwait(false);
            // ToDo: handle pagination
            if (assetList is null)
            {
                throw new Exception("Housing Search Service is not returning any asset list response");
            }

            var leaseholdAssets = new List<Asset>();
            // Hanna Holasava
            // We need to filter assets to be leasehold only for Dwelling type (for properties)
            // Estate and Blocks doesn't have Tenure enity
            if (housingSearchRequest.AssetType == Boundary.Request.Enums.AssetType.Dwelling)
            {
                leaseholdAssets.AddRange(GetLeaseholdersAssets(assetList.Results.Assets));
            }
            else
            {
                leaseholdAssets = assetList.Results.Assets;
            }

            var data = leaseholdAssets.Skip((housingSearchRequest.Page - 1) * housingSearchRequest.PageSize).Take(housingSearchRequest.PageSize);

            var assetTotals = new ConcurrentBag<PropertySearchResponse>();

            var degree = Convert.ToInt32(Math.Ceiling(Environment.ProcessorCount * 0.75 * 2.0));
            _logger.LogError("Degree Count : " + degree);
            var block = new ActionBlock<Asset>(
                    async x =>
                    {
                        Guid? tenureId = null;

                        if (Guid.TryParse(x.Tenure?.Id, out Guid result))
                        {
                            tenureId = result;
                        }

                        var assetTotal = new PropertySearchResponse()
                        {
                            AssetId = x.Id,
                            Address = x.AssetAddress,
                            TenureId = tenureId
                        };

                        try
                        {
                            List<Charge> detailCharge = await _getChargeByAssetIdUseCase.ExecuteAsync(x.Id).ConfigureAwait(false);

                            if (detailCharge != null)
                            {
                                var leaseholdCharges = detailCharge.Where(_ => _.ChargeGroup == ChargeGroup.Leaseholders);

                                for (var year = housingSearchRequest.FromYear; year <= DateTime.Now.Year; year++)
                                {
                                    assetTotal.Totals.Add(CalculateTotal(leaseholdCharges, year));
                                }
                            }

                            assetTotals.Add(assetTotal);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError("There is an error loading charges from ChargesAPI. Exception message " + ex.Message);
                        }
                    },
                    new ExecutionDataflowBlockOptions
                    {
                        MaxDegreeOfParallelism = degree, // Parallelize on all cores
                    });

            foreach (var asset in data)
            {
                block.Post(asset);
            }

            block.Complete();
            await block.Completion.ConfigureAwait(false);

            return new GetPropertyListResponse
            {
                Total = leaseholdAssets.Count,
                Assets = assetTotals.ToList()
            };
        }

        public static List<Asset> GetLeaseholdersAssets(List<Asset> assets)
        {
            if (assets == null || !assets.Any()) return new List<Asset>();

            var filteredData = assets.Where(
                   x => x.Tenure?.Type == TenureTypes.LeaseholdRTB.Description
                || x.Tenure?.Type == TenureTypes.PrivateSaleLH.Description
                || x.Tenure?.Type == TenureTypes.SharedOwners.Description
                || x.Tenure?.Type == TenureTypes.SharedEquity.Description
                || x.Tenure?.Type == TenureTypes.ShortLifeLse.Description
                || x.Tenure?.Type == TenureTypes.LeaseholdStair.Description
                || x.Tenure?.Type == TenureTypes.FreeholdServ.Description
            );

            return filteredData.ToList();
        }

        public static ChargesTotalResponse CalculateTotal(IEnumerable<Charge> charges, int year)
        {
            ChargeSubGroup subGroup = year >= DateTime.Now.Year - 1
                ? ChargeSubGroup.Estimate
                : ChargeSubGroup.Actual;

            var chargesToProcess = charges.Where(_ => _.ChargeYear == year
                                                   && _.ChargeSubGroup == subGroup);

            var detailedChargesToProcess = chargesToProcess.SelectMany(_ => _.DetailedCharges);

            var chargesTotal = new ChargesTotalResponse
            {
                Amount = detailedChargesToProcess.Any() ? detailedChargesToProcess.Sum(_ => _.Amount) : 0,
                Year = (short) year,
                Type = subGroup
            };

            return chargesTotal;
        }
    }
}
