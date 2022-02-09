using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using FinanceServicesApi.V1.Domain.Charges;
using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.Infrastructure;
using FinanceServicesApi.V1.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanceServicesApi.V1.Gateways
{
    public class ChargesGateway : IChargesGateway
    {
        // Hanna Holasava
        // All code that's related to AmazonDynamoDB is commentet out only for testing purposes.
        // We need to be able to uncomment it and debug the logic
        // private readonly IAmazonDynamoDB _amazonDynamoDb;
        private readonly IHousingData<List<Charge>> _housingData;

        public ChargesGateway(
            //IAmazonDynamoDB amazonDynamoDb,
            IHousingData<List<Charge>> housingData)
        {
            //_amazonDynamoDb = amazonDynamoDb;
            _housingData = housingData;
        }

        public async Task<List<Charge>> GetAllByAssetId(Guid assetId)
        {
            if (assetId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(assetId));
            }

            return await _housingData.DownloadAsync(assetId).ConfigureAwait(false);

            //QueryRequest request = new QueryRequest
            //{
            //    TableName = "Charges",
            //    KeyConditionExpression = "target_id = :V_target_id",
            //    ExpressionAttributeValues = new Dictionary<string, AttributeValue>
            //    {
            //        {":V_target_id",new AttributeValue{S = assetId.ToString()}}
            //    },
            //    ScanIndexForward = true
            //};

            //var response = await _amazonDynamoDb.QueryAsync(request).ConfigureAwait(false);
            //return response?.ToCharge();
        }
    }
}
