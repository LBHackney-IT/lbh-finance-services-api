using System;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BaseApi.V1.Boundary.Response;
using BaseApi.V1.Gateways.Interfaces.SuspenseTransaction;
using Newtonsoft.Json;

namespace BaseApi.V1.Gateways.SuspenseTransaction.Account
{
    public class TransactionGateway : ITransactionGateway
    {
        private readonly HttpClient _client;
        private readonly string _transactionApiUrl;
        private readonly string _transactionApiKey;

        public TransactionGateway()
        {
            _client = new HttpClient();
            _transactionApiUrl = Environment.GetEnvironmentVariable("TRANSACTION_API_URL");
            if (string.IsNullOrEmpty(_transactionApiUrl))
                throw new Exception("Transaction api url shouldn't be null");

            _transactionApiKey = Environment.GetEnvironmentVariable("TRANSACTION_API_KEY");
            if (string.IsNullOrEmpty(_transactionApiKey))
                throw new Exception("Transaction api key shouldn't be null");
        }

        public async Task<TransactionResponse> GetById(Guid id)
        {
            if (id == null || id == Guid.Empty)
                throw new NoNullAllowedException("The transaction id shouldn't be empty or null");

            _client.DefaultRequestHeaders.Add("x-api-key", _transactionApiKey);

            var response = await _client.GetAsync(new Uri($"{_transactionApiUrl}/{id.ToString()}")).ConfigureAwait(false);

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
