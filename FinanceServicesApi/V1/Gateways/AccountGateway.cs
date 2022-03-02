using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using FinanceServicesApi.V1.Boundary.Responses;
using FinanceServicesApi.V1.Domain.AccountModels;
using FinanceServicesApi.V1.Factories;
using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.Infrastructure.Entities;
using FinanceServicesApi.V1.Infrastructure.Enums;
using FinanceServicesApi.V1.Infrastructure.Interfaces;

namespace FinanceServicesApi.V1.Gateways
{
    public class AccountGateway : IAccountGateway
    {
        private readonly IDynamoDBContext _dynamoDbContext;
        private readonly IFinanceDomainApiData<GetAccountListResponse> _housingDataList;

        [ExcludeFromCodeCoverage]
        public AccountGateway(IDynamoDBContext dynamoDbContext,IFinanceDomainApiData<GetAccountListResponse> housingDataList)
        {
            _dynamoDbContext = dynamoDbContext;
            _housingDataList = housingDataList;
        }

        public async Task<Account> GetById(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException($"{nameof(id)} shouldn't be empty.");

            var result = await _dynamoDbContext.LoadAsync<AccountDbEntity>(id).ConfigureAwait(false);

            return result?.ToDomain();
        }

        public async Task<Account> GetByTargetId(Guid targetId)
        {
            if (targetId == Guid.Empty)
                throw new ArgumentException($"{nameof(targetId)} shouldn't be empty.");
            var result = await _housingDataList.DownloadAsync(targetId, SearchBy.ByTargetId).ConfigureAwait(false);

            if (result?.AccountResponseList == null || result.AccountResponseList.Count == 0)
                return null;
            return result.AccountResponseList[0];
        }
    }
}
