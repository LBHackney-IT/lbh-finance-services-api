using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Boundary.Responses;
using FinanceServicesApi.V1.Boundary.Responses.MetaData;
using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.Infrastructure.Enums;
using FinanceServicesApi.V1.Infrastructure.Interfaces;
using Hackney.Shared.HousingSearch.Domain.Accounts;
using Newtonsoft.Json;

namespace FinanceServicesApi.V1.Gateways
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

        public async Task<Account> GetById(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException($"the {nameof(id).ToString()} shouldn't be empty or null");

            var accountApiUrl = _getEnvironmentVariables.GetAccountApiUrl().ToString();
            var accountApiToken = _getEnvironmentVariables.GetAccountApiToken();

            _client.AddAuthorization(new AuthenticationHeaderValue("Bearer", accountApiToken));

            var response = await _client.GetAsync(new Uri($"{accountApiUrl}/accounts/{id.ToString()}")).ConfigureAwait(false);
            if (response == null)
            {
                throw new Exception($"The account api is not reachable!{accountApiUrl}");
            }
            else if (response.Content == null)
            {
                throw new Exception(response.StatusCode.ToString());
            }
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            Account accountResponse = JsonConvert.DeserializeObject<Account>(responseContent);
            return accountResponse;
        }

        public async Task<List<Account>> GetByTargetId(Guid targetId)
        {
            if (targetId == Guid.Empty)
                throw new ArgumentNullException($"the {nameof(targetId).ToString()} shouldn't be empty or null");

            var searchApiUrl = _getEnvironmentVariables.GetHousingSearchApi(ESearchBy.ByTransaction).ToString();
            var searchAuthKey = _getEnvironmentVariables.GetHousingSearchApiToken();

            _client.AddHeader(new HttpHeader<string, string> { Name = "Authorization", Value = searchAuthKey });

            var response = await _client.GetAsync(new Uri($"{searchApiUrl}?TargetId=${targetId.ToString()}")).ConfigureAwait(false);
            if (response == null)
            {
                throw new Exception("The search api is not reachable!");
            }
            else if (response.Content == null)
            {
                throw new Exception(response.StatusCode.ToString());
            }
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var transactionResponse = JsonConvert.DeserializeObject<APIResponse<HousingSearchResponse<Account>>>(responseContent);
            return transactionResponse?.Results.ResponseList;
        }
    }
}
