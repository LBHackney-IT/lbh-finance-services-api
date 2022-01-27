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
        private readonly IAmazonDynamoDB _amazonDynamoDb;

        public ChargesGateway(IAmazonDynamoDB amazonDynamoDb)
        {
            _amazonDynamoDb = amazonDynamoDb;
        }

        public async Task<List<Charge>> GetAllByAssetId(Guid assetId)
        {
            if (assetId == Guid.Empty)
                throw new ArgumentException($"{nameof(assetId).ToString()} shouldn't be empty.");

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
