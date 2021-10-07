using System;
using System.Data;
using System.Net.Http;
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
        private readonly string _accountApiUrl;
        private readonly string _accountApiToken;
        public AccountGateway(ICustomeHttpClient client)
        {
            _client = client;
            _accountApiUrl = Environment.GetEnvironmentVariable("ACCOUNT_API_URL");
            if (string.IsNullOrEmpty(_accountApiUrl))
                throw new Exception("Account api url shouldn't be null");

            _accountApiToken = Environment.GetEnvironmentVariable("ACCOUNT_API_TOKEN");
            if (string.IsNullOrEmpty(_accountApiToken))
                throw new Exception("Account api token shouldn't be null");
        }

        public async Task<AccountResponse> GetById(Guid id)
        {
            if (id == null || id == Guid.Empty)
                throw new ArgumentNullException($"the {nameof(id).ToString()} shouldn't be empty or null");

            _client.AddAuthorization(new AuthenticationHeaderValue("Bearer", _accountApiToken));

            var response = await _client.GetAsync(new Uri($"{_accountApiUrl}/{id.ToString()}")).ConfigureAwait(false);
            if (response == null)
            {
                throw new Exception("The account api is not reachable!");
            }
            else if (response.Content == null)
            {
                throw new Exception(response.StatusCode.ToString());
            }
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (responseContent != null)
            {
                AccountResponse accountResponse = JsonConvert.DeserializeObject<AccountResponse>(responseContent);
                return accountResponse;
            }
            else
                throw new Exception("The account doesn't exists");
        }
    }
}
