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
    public class TransactionGateway : ITransactionGateway
    {
        private readonly ICustomeHttpClient _client;
        private readonly string _transactionApiUrl;
        private readonly string _transactionApiKey;

        public TransactionGateway(ICustomeHttpClient client, IEnvironmentVariables environmentVariables)
        {
            _client = client;
            _transactionApiUrl = environmentVariables.GetTransactionApiUrl().ToString();
            if (string.IsNullOrEmpty(_transactionApiUrl))
                throw new Exception("Transaction api url shouldn't be null");

            _transactionApiKey = environmentVariables.GetTransactionApiKey();
            if (string.IsNullOrEmpty(_transactionApiKey))
                throw new Exception("Transaction api key shouldn't be null");
        }

        public async Task<TransactionResponse> GetById(Guid id)
        {
            /*if (id == null || id == Guid.Empty)
                throw new NoNullAllowedException($"The {nameof(id).ToString()} shouldn't be empty or null");

            _client.AddHeader(new HttpHeader<string, string>{ Name = "x-api-key", Value = _transactionApiKey });

            var response = await _client.GetAsync(new Uri($"{_transactionApiUrl}/{id.ToString()}")).ConfigureAwait(false);

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (responseContent != null)
            {
                TransactionResponse transactionResponse = JsonConvert.DeserializeObject<TransactionResponse>(responseContent);
                if (!transactionResponse.IsSuspense)
                    throw new Exception("Transaction is not suspense");
                return transactionResponse;
            }
            else
                throw new Exception("The transaction doesn't exists");*/
            if (id == null || id == Guid.Empty)
                throw new ArgumentNullException($"the {nameof(id).ToString()} shouldn't be empty or null");

            _client.AddHeader(new HttpHeader<string, string> { Name = "x-api-key", Value = _transactionApiKey });

            var response = await _client.GetAsync(new Uri($"{_transactionApiUrl}/{id.ToString()}")).ConfigureAwait(false);
            if (response == null)
            {
                throw new Exception("The transaction api is not reachable!");
            }
            else if (response.Content == null)
            {
                throw new Exception(response.StatusCode.ToString());
            }
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (responseContent != null)
            {
                TransactionResponse transactionResponse = JsonConvert.DeserializeObject<TransactionResponse>(responseContent);
                return transactionResponse;
            }
            else
                throw new Exception("The transaction doesn't exists");
        }
    }
}
