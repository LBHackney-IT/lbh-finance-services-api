using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace FinanceServicesApi.V1.Infrastructure
{
    public class HousingData<T> : IHousingData<T>
        where T : class
    {
        private readonly ICustomeHttpClient _client;
        private readonly IGetEnvironmentVariables<T> _getEnvironmentVariables;
        private readonly IGenerateUrl<T> _generateUrl;

        public HousingData(ICustomeHttpClient client, IGetEnvironmentVariables<T> getEnvironmentVariables, IGenerateUrl<T> generateUrl)
        {
            _client = client;
            _getEnvironmentVariables = getEnvironmentVariables;
            _generateUrl = generateUrl;
        }

        public async Task<T> DownloadAsync(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentNullException(nameof(id));

            var apiToken = _getEnvironmentVariables.GetToken();

            _client.AddAuthorization(new AuthenticationHeaderValue("Bearer", apiToken));
            Uri uri = _generateUrl.Execute(id);

            var response = await _client.GetAsync(uri).ConfigureAwait(false);
            if (response == null)
            {
                throw new Exception($"The {nameof(T)} api is not reachable!");
            }
            else if (response.Content == null)
            {
                throw new Exception(response.StatusCode.ToString());
            }

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            throw new Exception(responseContent);

            /*if (!response.IsSuccessStatusCode)
                throw new Exception(responseContent);

            var tResponse = JsonConvert.DeserializeObject<T>(responseContent);
            return tResponse;*/
        }
    }
}
