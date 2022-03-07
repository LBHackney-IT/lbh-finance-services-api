using FinanceServicesApi.V1.Boundary.Request;
using FinanceServicesApi.V1.Boundary.Request.Enums;
using FinanceServicesApi.V1.Boundary.Responses;
using FinanceServicesApi.V1.Boundary.Responses.MetaData;
using FinanceServicesApi.V1.Domain.AssetModels;
using FinanceServicesApi.V1.Gateways.Extensions;
using FinanceServicesApi.V1.Gateways.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FinanceServicesApi.V1.Gateways
{
    public class HousingSearchGateway : IHousingSearchGateway
    {
        private readonly HttpClient _client;
        private readonly ILogger<HousingSearchGateway> _logger;

        public HousingSearchGateway(HttpClient client, ILogger<HousingSearchGateway> logger)
        {
            _client = client;
            _logger = logger;
        }
        public async Task<AssetListResponse> GetAssets(string searchText, AssetType assetType)
        {
            var uri = new Uri($"api/v1/search/assets/all?sortBy=assetId&isDesc=false&assetTypes={assetType.ToString()}&searchText={searchText}&pageSize=7000&page=1", UriKind.Relative);

            var response = await _client.GetAsync(uri).ConfigureAwait(true);
            var result = await response.ReadContentAs<AssetListResponse>().ConfigureAwait(true);
            return result;
        }

        public async Task<GetPersonListResponse> GetPersons(GetPersonListRequest request)
        {
            var uri = new Uri(GeneratePersonSearchUrl(request), UriKind.Relative);
            var apiResponse = await _client.GetAsync(uri).ConfigureAwait(true);

            if(!apiResponse.IsSuccessStatusCode)
            {
                var errorResponseMessage = await apiResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                _logger.LogError("Failed to get persons from HousingSearchAPI");
                _logger.LogError(errorResponseMessage);

                throw new Exception(apiResponse.StatusCode.ToString() + ". Failed to get persons from HousingSearchAPI");
            }

            var personsList = await apiResponse.ReadContentAs<APIResponse<GetPersonListResponse>>().ConfigureAwait(true);
            return personsList.Results;
        }

        private static string GeneratePersonSearchUrl(GetPersonListRequest request)
        {
            string urlPath = $"api/v1/search/persons?searchText={request.SearchText}&pageSize={request.PageSize}&page={request.Page}&sortBy={request.SortBy}&isDesc={request.IsDesc}";
            if (request.PersonType.HasValue)
            {
                urlPath += $"&personType={request.PersonType.Value}";
            }

            return urlPath;
        }
    }
}
