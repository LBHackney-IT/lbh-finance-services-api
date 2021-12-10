using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using FinanceServicesApi.V1.Domain.Charges;
using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.Infrastructure;
using FinanceServicesApi.V1.Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace FinanceServicesApi.V1.Gateways
{
    public class ChargesGateway : IChargesGateway
    {
        private readonly ICustomeHttpClient _client;
        private readonly IAmazonDynamoDB _amazonDynamoDb;
        private readonly IGetEnvironmentVariables _getEnvironmentVariables;

        public ChargesGateway(ICustomeHttpClient client, IAmazonDynamoDB amazonDynamoDb, IGetEnvironmentVariables getEnvironmentVariables)
        {
            _client = client;
            _amazonDynamoDb = amazonDynamoDb;
            _getEnvironmentVariables = getEnvironmentVariables;
        }
        public async Task<List<Charge>> GetAllByAssetId(Guid assetId)
        {
            if (assetId == Guid.Empty)
                throw new ArgumentNullException($"the {nameof(assetId).ToString()} shouldn't be empty or null");

            QueryRequest request = new QueryRequest
            {
                TableName = "Charges",
                /*IndexName = "target_id",*/
                KeyConditionExpression = "target_id = :V_target_id",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {":V_target_id",new AttributeValue{S = assetId.ToString()}}
                },
                ScanIndexForward = true
            };

            var response = await _amazonDynamoDb.QueryAsync(request).ConfigureAwait(false);
            List<Charge> data = response.ToCharge();

            return data;

            /*var chargesApiUrl = _getEnvironmentVariables.GetChargesApiUrl().ToString();
            var chargesApiKey = _getEnvironmentVariables.GetChargesApiKey();

            _client.AddHeader(new HttpHeader<string, string> { Name = "x-api-key", Value = chargesApiKey });

            var response = await _client.GetAsync(new Uri($"{chargesApiUrl}/api/v1/charges?type={targetType}&targetId={targetId.ToString()}")).ConfigureAwait(false);
            if (response == null)
            {
                throw new Exception("The charges api is not reachable!");
            }
            else if (response.Content == null)
            {
                throw new Exception(response.StatusCode.ToString());
            }
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var chargesResponse = JsonConvert.DeserializeObject<Charge>(responseContent);
            return chargesResponse;*/

        }
    }
}
