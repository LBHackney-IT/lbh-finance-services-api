using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using FinanceServicesApi.V1.Domain.AccountModels;
using FinanceServicesApi.V1.Factories;
using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.Infrastructure;
using FinanceServicesApi.V1.Infrastructure.Entities;
using FinanceServicesApi.V1.Infrastructure.Enums;

namespace FinanceServicesApi.V1.Gateways
{
    public class AccountGateway : IAccountGateway
    {
        private readonly IDynamoDBContext _dynamoDbContext;
        private readonly AmazonDynamoDBClient _amazonDynamoDb;

        public AccountGateway(IDynamoDBContext dynamoDbContext,AmazonDynamoDBClient amazonDynamoDb)
        {
            _dynamoDbContext = dynamoDbContext;
            _amazonDynamoDb = amazonDynamoDb;
        }

        public async Task<Account> GetById(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException($"the {nameof(id).ToString()} shouldn't be empty or null");

            var result = await _dynamoDbContext.LoadAsync<AccountDbEntity>(id).ConfigureAwait(false);

            return result?.ToDomain();
        }

        public async Task<List<Account>> GetByTargetId(Guid targetId)
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

            return response.ToAccounts();
        }
    }
}
