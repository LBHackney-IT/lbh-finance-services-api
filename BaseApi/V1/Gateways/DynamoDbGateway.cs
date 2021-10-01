using System;
using Amazon.DynamoDBv2.DataModel;
using BaseApi.V1.Domain;
using BaseApi.V1.Factories;
using BaseApi.V1.Infrastructure;
using System.Collections.Generic;
using BaseApi.V1.Domain.SuspenseTransaction;

namespace BaseApi.V1.Gateways
{
    public class DynamoDbGateway : IExampleGateway
    {
        private readonly IDynamoDBContext _dynamoDbContext;

        public DynamoDbGateway(IDynamoDBContext dynamoDbContext)
        {
            _dynamoDbContext = dynamoDbContext;
        }

        public List<ConfirmTransferEntity> GetAll()
        {
            return new List<ConfirmTransferEntity>();
        }

        public ConfirmTransferEntity GetEntityById(Guid id)
        {
            var result = _dynamoDbContext.LoadAsync<DatabaseEntity>(id).GetAwaiter().GetResult();
            return result?.ToDomain();
        }
    }
}
