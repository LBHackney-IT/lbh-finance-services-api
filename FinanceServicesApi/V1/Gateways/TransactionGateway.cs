using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using FinanceServicesApi.V1.Boundary.Request;
using FinanceServicesApi.V1.Domain.TransactionModels;
using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.Infrastructure.Enums;
using FinanceServicesApi.V1.Infrastructure.Interfaces;

namespace FinanceServicesApi.V1.Gateways
{
    public class TransactionGateway : ITransactionGateway
    {
        private readonly IAmazonDynamoDB _amazonDynamoDb;
        private readonly IDynamoDBContext _dynamoDbContext;
        private readonly IHousingData<Transaction> _housingData;
        private readonly IHousingData<List<Transaction>> _iHousingData;

        public TransactionGateway(IAmazonDynamoDB amazonDynamoDb,
            IDynamoDBContext dynamoDbContext,
            IHousingData<Transaction> housingData,
            IHousingData<List<Transaction>> iHousingData)
        {
            _amazonDynamoDb = amazonDynamoDb;
            _dynamoDbContext = dynamoDbContext;
            _housingData = housingData;
            _iHousingData = iHousingData;
        }

        public async Task<Transaction> GetById(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException($"{nameof(id).ToString()} shouldn't be empty.");

            return await _housingData.DownloadAsync(id).ConfigureAwait(false);
            /*if (id == Guid.Empty)
                throw new ArgumentException($"{nameof(id).ToString()} shouldn't be empty.");

            var response = await _dynamoDbContext.LoadAsync<TransactionDbEntity>(Guid.Empty, id).ConfigureAwait(false);

            if (response == null)
            {
                throw new Exception("The transaction api is not reachable!");
            }

            return response.ToDomain();*/
        }

        public async Task<List<Transaction>> GetByTargetId(TransactionsRequest transactionsRequest)
        {
            if (transactionsRequest == null || transactionsRequest.TargetId == Guid.Empty)
                throw new ArgumentException($"{nameof(transactionsRequest.TargetId).ToString()} shouldn't be empty.");

            return await _iHousingData.DownloadAsync(transactionsRequest.TargetId, SearchBy.ByTargetId).ConfigureAwait(false);

            /*if (transactionsRequest.TargetId == Guid.Empty)
                throw new ArgumentException($"{nameof(transactionsRequest.TargetId).ToString()} shouldn't be empty.");

            QueryRequest request = new QueryRequest
            {
                TableName = "Transactions",
                KeyConditionExpression = "target_id = :V_target_id",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {":V_target_id",new AttributeValue{S = transactionsRequest.TargetId.ToString()}}
                },
                ScanIndexForward = true
            };

            var response = await _amazonDynamoDb.QueryAsync(request).ConfigureAwait(false);
            return response?.ToTransactions();*/
        }
    }
}
