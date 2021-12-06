using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Boundary.Responses;
using FinanceServicesApi.V1.Boundary.Responses.MetaData;
using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.Infrastructure.Interfaces;
using Hackney.Shared.HousingSearch.Domain.Transactions;
using Newtonsoft.Json;

namespace FinanceServicesApi.V1.Gateways
{
    public class TransactionGateway : ITransactionGateway
    {
        private readonly ICustomeHttpClient _client;
        private readonly IGetEnvironmentVariables _getEnvironmentVariables;

        public TransactionGateway(ICustomeHttpClient client, IGetEnvironmentVariables getEnvironmentVariables)
        {
            _client = client;
            _getEnvironmentVariables = getEnvironmentVariables;
        }

        public async Task<Transaction> GetById(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException($"the {nameof(id).ToString()} shouldn't be empty or null");

            var transactionApiUrl = _getEnvironmentVariables.GetTransactionApiUrl().ToString();
            var transactionApiKey = _getEnvironmentVariables.GetTransactionApiKey();

            _client.AddHeader(new HttpHeader<string, string> { Name = "x-api-key", Value = transactionApiKey });

            var response = await _client.GetAsync(new Uri($"{transactionApiUrl}/transactions/{id.ToString()}")).ConfigureAwait(false);
            if (response == null)
            {
                throw new Exception("The transaction api is not reachable!");
            }
            else if (response.Content == null)
            {
                throw new Exception(response.StatusCode.ToString());
            }
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            Transaction transactionResponse = JsonConvert.DeserializeObject<Transaction>(responseContent);
            return transactionResponse;
        }

        public async Task<List<Transaction>> GetByTargetId(Guid targetId)
        {
            if (targetId == Guid.Empty)
                throw new ArgumentNullException($"the {nameof(targetId).ToString()} shouldn't be empty or null");

            var searchApiUrl = _getEnvironmentVariables.GetSearchApiUrl().ToString();
            var searchAuthKey = _getEnvironmentVariables.GetTransactionApiKey();

            _client.AddHeader(new HttpHeader<string, string> { Name = "Authorization", Value = searchAuthKey });

            var response = await _client.GetAsync(new Uri($"{searchApiUrl}/search/transactions?TargetId=${targetId.ToString()}")).ConfigureAwait(false);
            if (response == null)
            {
                throw new Exception("The search api is not reachable!");
            }
            else if (response.Content == null)
            {
                throw new Exception(response.StatusCode.ToString());
            }
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var transactionResponse = JsonConvert.DeserializeObject<APIResponse<HousingSearchResponse<Transaction>>>(responseContent);
            return transactionResponse?.Results.ResponseList;
        }
    }
}
