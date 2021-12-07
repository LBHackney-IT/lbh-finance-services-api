using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.Infrastructure.Interfaces;
using Hackney.Shared.Tenure.Domain;
using Newtonsoft.Json;

namespace FinanceServicesApi.V1.Gateways
{
    public class TenureInformationGateway : ITenureInformationGateway
    {
        private readonly ICustomeHttpClient _client;
        private readonly IGetEnvironmentVariables _getEnvironmentVariables;

        public TenureInformationGateway(ICustomeHttpClient client, IGetEnvironmentVariables getEnvironmentVariables)
        {
            _client = client;
            _getEnvironmentVariables = getEnvironmentVariables;
        }
        public async Task<TenureInformation> GetById(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException($"the {nameof(id).ToString()} shouldn't be empty or null");

            var tenureInformationApiUrl = _getEnvironmentVariables.GetTenureInformationApiUrl().ToString();
            var tenureInformationApiToken = _getEnvironmentVariables.GetTenureInformationApiToken();

            _client.AddAuthorization(new AuthenticationHeaderValue("Bearer", tenureInformationApiToken));

            var response = await _client.GetAsync(new Uri($"{tenureInformationApiUrl}/api/v1/tenures/{id.ToString()}")).ConfigureAwait(false);
            if (response == null)
            {
                throw new Exception("The tenure information api is not reachable!");
            }
            else if (response.Content == null)
            {
                throw new Exception(response.StatusCode.ToString());
            }
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var tenureInformationResponse = JsonConvert.DeserializeObject<TenureInformation>(responseContent);
            return tenureInformationResponse;
        }
    }
}
