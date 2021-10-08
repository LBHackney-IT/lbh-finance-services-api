using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BaseApi.V1.Boundary.Response;
using BaseApi.V1.Gateways.Interfaces.SuspenseTransaction;
using BaseApi.V1.Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace BaseApi.V1.Gateways.SuspenseTransaction
{
    public class AccountGateway : IAccountGateway
    {
        private readonly ICustomeHttpClient _client;
        private readonly IGetEnvironmentVariables _getEnvironmentVariables;

        public AccountGateway(ICustomeHttpClient client, IGetEnvironmentVariables getEnvironmentVariables)
        {
            _client = client;
            _getEnvironmentVariables = getEnvironmentVariables;
        }

        public async Task<AccountResponse> GetById(Guid id)
        {
            if (id == null || id == Guid.Empty)
                throw new ArgumentNullException($"the {nameof(id).ToString()} shouldn't be empty or null");

            var accountApiUrl = _getEnvironmentVariables.GetAccountApiUrl().ToString();
            var accountApiToken = _getEnvironmentVariables.GetAccountApiToken();

            _client.AddAuthorization(new AuthenticationHeaderValue("Bearer", accountApiToken));

            var response = await _client.GetAsync(new Uri($"{accountApiUrl}/{id.ToString()}")).ConfigureAwait(false);
            if (response == null)
            {
                throw new Exception("The account api is not reachable!");
            }
            else if (response.Content == null)
            {
                throw new Exception(response.StatusCode.ToString());
            }
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                AccountResponse accountResponse = JsonConvert.DeserializeObject<AccountResponse>(responseContent);
                return accountResponse;
        }
    }
}
