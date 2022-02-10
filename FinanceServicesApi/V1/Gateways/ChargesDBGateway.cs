using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using FinanceServicesApi.V1.Domain.Charges;
using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanceServicesApi.V1.Gateways
{
    public class ChargesDBGateway : IChargesGateway
    {
        private readonly IAmazonDynamoDB _amazonDynamoDb;

        public ChargesDBGateway(IAmazonDynamoDB amazonDynamoDb)
        {
            _amazonDynamoDb = amazonDynamoDb;
        }

        public async Task<List<Charge>> GetAllByAssetId(Guid assetId)
        {
            QueryRequest request = new QueryRequest
            {
                TableName = "Charges",
                KeyConditionExpression = "target_id = :V_target_id",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {":V_target_id",new AttributeValue{S = assetId.ToString()}}
                },
                ScanIndexForward = true
            };

            var response = await _amazonDynamoDb.QueryAsync(request).ConfigureAwait(false);
            return response?.ToCharge();
        }
    }
}
