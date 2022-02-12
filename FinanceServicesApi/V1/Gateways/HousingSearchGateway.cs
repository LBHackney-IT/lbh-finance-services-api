using FinanceServicesApi.V1.Boundary.Request.Enums;
using FinanceServicesApi.V1.Domain.AssetModels;
using FinanceServicesApi.V1.Gateways.Extensions;
using FinanceServicesApi.V1.Gateways.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FinanceServicesApi.V1.Gateways
{
    public class HousingSearchGateway : IHousingSearchGateway
    {
        private readonly HttpClient _client;
        public HousingSearchGateway(HttpClient client)
        {
            _client = client;
        }
        public async Task<AssetListResponse> GetAssets(string searchText, AssetType assetType)
        {
            var uri = new Uri($"api/v1/search/assets/all?sortBy=assetId&isDesc=false&assetTypes={assetType.ToString()}&searchText={searchText}&pageSize=7000&page=1", UriKind.Relative);

            var response = await _client.GetAsync(uri).ConfigureAwait(true);
            var result = await response.ReadContentAs<AssetListResponse>().ConfigureAwait(true);
            return result;
        }
    }
}
