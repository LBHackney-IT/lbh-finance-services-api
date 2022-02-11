using FinanceServicesApi.V1.Boundary.Request;
using FinanceServicesApi.V1.Boundary.Responses;
using FinanceServicesApi.V1.Boundary.Responses.PropertySummary;
using FinanceServicesApi.V1.Domain.Charges;
using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.Infrastructure.Enums;
using FinanceServicesApi.V1.UseCase.Interfaces;
using Hackney.Shared.Asset.Domain;
using Hackney.Shared.Tenure.Domain;
using System;
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

        public GetLeaseholdAssetsListUseCase(IHousingSearchGateway housingSearchGateway,
            IGetChargeByAssetIdUseCase getChargeByAssetIdUseCase)
        {
            _housingSearchGateway = housingSearchGateway;
            _getChargeByAssetIdUseCase = getChargeByAssetIdUseCase;
        }

        public async Task<GetPropertyListResponse> ExecuteAsync(LeaseholdAssetsRequest housingSearchRequest)
        {
            var leaseholdAssets = new List<Asset>();

            var assetList = await _housingSearchGateway.GetAssets(housingSearchRequest.SearchText, housingSearchRequest.AssetType).ConfigureAwait(false);
            // ToDo: handle pagination
            if (assetList is null)
            {
                throw new Exception("Housing Search Service is not returning any asset list response");
            }

            leaseholdAssets.AddRange(GetLeaseholdersAssets(assetList.Results.Assets));

            var data = leaseholdAssets.Skip((housingSearchRequest.Page - 1) * housingSearchRequest.PageSize).Take(housingSearchRequest.PageSize);

            var assetTotals = new List<PropertySearchResponse>();

            var degree = Convert.ToInt32(Math.Ceiling((Environment.ProcessorCount * 0.75) * 2.0));
            var block = new ActionBlock<Asset>(
                    async x =>
                    {
                        var assetTotal = new PropertySearchResponse()
                        {
                            AssetId = x.Id,
                            Address = x.AssetAddress,
                            TenureId = Guid.Parse(x.Tenure?.Id),
                        };

                        var detailCharge = await _getChargeByAssetIdUseCase.ExecuteAsync(x.Id).ConfigureAwait(false);

                        if (detailCharge != null && detailCharge.Any())
                        {
                            var leaseholdCharges = detailCharge.Where(_ => _.ChargeGroup == ChargeGroup.Leaseholders);

                            assetTotal.Totals.Add(CalculateTotal(leaseholdCharges, housingSearchRequest.Year - 2, ChargeSubGroup.Actual));
                            assetTotal.Totals.Add(CalculateTotal(leaseholdCharges, housingSearchRequest.Year - 1, ChargeSubGroup.Estimate));
                            assetTotal.Totals.Add(CalculateTotal(leaseholdCharges, housingSearchRequest.Year, ChargeSubGroup.Estimate));
                        }

                        assetTotals.Add(assetTotal);
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
                Assets = assetTotals
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

        public static ChargesTotalResponse CalculateTotal(IEnumerable<Charge> charges, int year, ChargeSubGroup group)
        {
            var chargesToProcess = charges.Where(_ => _.ChargeYear == year
                                                   && _.ChargeSubGroup == group);

            var detailedChargesToProcess = chargesToProcess.SelectMany(_ => _.DetailedCharges);

            var chargesTotal = new ChargesTotalResponse
            {
                Amount = Convert.ToInt32(detailedChargesToProcess.Any() ? detailedChargesToProcess.Sum(_ => _.Amount) : 0),
                Year = (short) year,
                Type = group
            };

            return chargesTotal;
        }
    }
}
