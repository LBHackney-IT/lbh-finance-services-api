using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Boundary.Request;
using FinanceServicesApi.V1.Boundary.Responses;
using FinanceServicesApi.V1.Boundary.Responses.MetaData;
using FinanceServicesApi.V1.Domain.TransactionModels;
using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.Infrastructure.Enums;
using FinanceServicesApi.V1.Infrastructure.Interfaces;
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

            _client.AddAuthorization(new AuthenticationHeaderValue("x-api-key", transactionApiKey));

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

        public async Task<List<Transaction>> GetByTargetId(TransactionsRequest transactionsRequest)
        {
            if (transactionsRequest.TargetId == Guid.Empty)
                throw new ArgumentNullException($"the {nameof(transactionsRequest.TargetId).ToString()} shouldn't be empty or null");

            var searchApiUrl = _getEnvironmentVariables.GetHousingSearchApi(SearchBy.ByTransaction).ToString();
            var searchAuthKey = _getEnvironmentVariables.GetHousingSearchApiToken();

            _client.AddHeader(new HttpHeader<string, string> { Name = "Authorization", Value = searchAuthKey });
            //_client.AddAuthorization(new AuthenticationHeaderValue("Authorization", searchAuthKey));

            var response = await _client.GetAsync(new Uri($"{searchApiUrl}?" +
                                                          $"TargetId={transactionsRequest.TargetId.ToString()}&" +
                                                          $"Page={transactionsRequest.Page}&" +
                                                          $"PageSize={transactionsRequest.PageSize}&" +
                                                          $"SortBy={transactionsRequest.SortBy}&" +
                                                          $"IsDesc={transactionsRequest.IsDesc}")).ConfigureAwait(false);
            if (response == null)
            {
                throw new Exception("The search api is not reachable!");
            }
            else if (response.Content == null)
            {
                throw new Exception(response.StatusCode.ToString());
            }
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var transactionResponse = JsonConvert.DeserializeObject<APIResponse<GetTransactionListResponse>>(responseContent);
            return transactionResponse?.Results.Transactions;
        }
    }
}
