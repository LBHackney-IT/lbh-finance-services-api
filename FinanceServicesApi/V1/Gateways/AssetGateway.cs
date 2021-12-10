using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.Infrastructure.Interfaces;
using Hackney.Shared.Asset.Domain;
using Hackney.Shared.Tenure.Domain;
using Newtonsoft.Json;

namespace FinanceServicesApi.V1.Gateways
{
    public class AssetGateway: IAssetGateway
    {
        private readonly ICustomeHttpClient _client;
        private readonly IGetEnvironmentVariables _getEnvironmentVariables;

        public AssetGateway(ICustomeHttpClient client,IGetEnvironmentVariables getEnvironmentVariables)
        {
            _client = client;
            _getEnvironmentVariables = getEnvironmentVariables;
        }
        public async Task<Asset> GetById(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException($"the {nameof(id).ToString()} shouldn't be empty or null");

            var assetInformationApiUrl = _getEnvironmentVariables.GetAssetInformationApiUrl().ToString();
            var assetInformationApiToken = _getEnvironmentVariables.GetAssetInformationApiToken();

            _client.AddAuthorization(new AuthenticationHeaderValue("Bearer", assetInformationApiToken));

            var response = await _client.GetAsync(new Uri($"{assetInformationApiUrl}/api/v1/assets/{id.ToString()}")).ConfigureAwait(false);
            if (response == null)
            {
                throw new Exception("The asset api is not reachable!");
            }
            else if (response.Content == null)
            {
                throw new Exception(response.StatusCode.ToString());
            }
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var assetResponse = JsonConvert.DeserializeObject<Asset>(responseContent);
                return assetResponse;
            }

            return null;
        }
    }
}
