using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using FinanceServicesApi.V1.Domain.AccountModels;
using FinanceServicesApi.V1.Factories;
using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.Infrastructure.Entities;
using FinanceServicesApi.V1.Infrastructure.Interfaces;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace FinanceServicesApi.V1.Gateways
{
    public class AccountGateway : IAccountGateway
    {
        private readonly IDynamoDBContext _dynamoDbContext;
        private readonly IAmazonDynamoDB _amazonDynamoDb;
        private readonly IHousingData<Account> _housingData;

        [ExcludeFromCodeCoverage]
        public AccountGateway(IDynamoDBContext dynamoDbContext, IAmazonDynamoDB amazonDynamoDb, IHousingData<Account> housingData)
        {
            _dynamoDbContext = dynamoDbContext;
            _amazonDynamoDb = amazonDynamoDb;
            _housingData = housingData;
        }

        public async Task<Account> GetById(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(id)} shouldn't be empty.");
            }

            var result = await _dynamoDbContext.LoadAsync<AccountDbEntity>(id).ConfigureAwait(false);

            return result?.ToDomain();
        }

        public async Task<Account> GetByTargetId(Guid targetId)
        {
            if (targetId == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(targetId)} shouldn't be empty.");
            }

            return await _housingData.DownloadAsync(targetId).ConfigureAwait(false);
        }


        /*public async Task<Account> GetByTargetId(Guid targetId)
        {
            QueryRequest request = new QueryRequest
            {
                TableName = "Accounts",
                IndexName = "target_id_dx",
                KeyConditionExpression = "target_id = :V_target_id",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {":V_target_id",new AttributeValue{S = targetId.ToString()}}
                },
                ScanIndexForward = true
            };

            var response = await _amazonDynamoDb.QueryAsync(request).ConfigureAwait(false);

            return response?.ToAccount();
        }*/
    }
}
