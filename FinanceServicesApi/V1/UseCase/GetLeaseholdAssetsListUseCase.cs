using FinanceServicesApi.V1.Boundary.Request;
using FinanceServicesApi.V1.Boundary.Request.MetaData;
using FinanceServicesApi.V1.Boundary.Responses;
using FinanceServicesApi.V1.Boundary.Responses.PropertySummary;
using FinanceServicesApi.V1.Gateways.Interfaces;
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
            var filteredList = new List<Asset>();

            var assetList = await _housingSearchGateway.GetAssets(housingSearchRequest.SearchText).ConfigureAwait(false);

            if (assetList is null) throw new Exception("Housing Search Service is not returning any asset list response");

            filteredList.AddRange(GetLeaseholdersAssets(assetList.Results.Assets));

            var totalPropertiesCount = filteredList.Count;

            var data = filteredList.Skip((housingSearchRequest.Page - 1) * housingSearchRequest.PageSize).Take(housingSearchRequest.PageSize);


            var properties = new List<PropertySearchResponse>();

            var degree = Convert.ToInt32(Math.Ceiling((Environment.ProcessorCount * 0.75) * 2.0));

            var block = new ActionBlock<Asset>(
                    async x =>
                    {
                        var totalCharge = 0;
                        var detailCharge = await _getChargeByAssetIdUseCase.ExecuteAsync(x.Id).ConfigureAwait(false);
                        if (detailCharge != null && detailCharge.Any())
                        {
                            var leaseholdData = detailCharge.FirstOrDefault(_ =>
                            _.ChargeGroup == Infrastructure.Enums.ChargeGroup.Leaseholders &&
                            _.ChargeYear == housingSearchRequest.Year);

                            if (leaseholdData != null)
                            {
                                totalCharge = Convert.ToInt32(leaseholdData.DetailedCharges.Sum(_ => _.Amount));
                            }
                        }

                        var resultData = new PropertySearchResponse
                        {
                            AssetId = x.Id,
                            Address = x.AssetAddress,
                            TotalEstimateAmount = totalCharge
                        };
                        properties.Add(resultData);
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
                Total = totalPropertiesCount,
                Properties = properties
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
    }
}
