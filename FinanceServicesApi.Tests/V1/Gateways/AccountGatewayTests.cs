using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using AutoFixture;
using FinanceServicesApi.Tests.V1.Helper;
using FinanceServicesApi.V1.Domain.AccountModels;
using FinanceServicesApi.V1.Domain.Charges;
using FinanceServicesApi.V1.Factories;
using FinanceServicesApi.V1.Gateways;
using FinanceServicesApi.V1.Infrastructure;
using FinanceServicesApi.V1.Infrastructure.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace FinanceServicesApi.Tests.V1.Gateways
{
    public class AccountGatewayTests
    {
        private readonly Mock<IDynamoDBContext> _dynamoDbContext;
        private readonly Mock<IAmazonDynamoDB> _amazonDynamoDb;
        private readonly Fixture _fixture;
        private AccountGateway _sut;

        public AccountGatewayTests()
        {
            _fixture = new Fixture();
            _dynamoDbContext = new Mock<IDynamoDBContext>();
            _amazonDynamoDb = new Mock<IAmazonDynamoDB>();
            _sut = new AccountGateway(_dynamoDbContext.Object,_amazonDynamoDb.Object);
        }

        /*[Fact]
        public void GetByIdWithEmptyInputReturnsException()
        {
            Func<Task<Account>> func = async () => await _sut.GetById(Guid.Empty).ConfigureAwait(false);
            func.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void GetByIdWitValidInputReturnsData()
        {
            AccountDbEntity response = _fixture.Create<AccountDbEntity>();

            _dynamoDbContext.Setup(_ => _.LoadAsync<AccountDbEntity>(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(response);

            Func<Task<Account>> func = async () => await _sut.GetById(Guid.NewGuid()).ConfigureAwait(false);

            var result = func.Invoke();
            result.Should().NotBeNull();
            result.Result.Should().BeEquivalentTo(response.ToDomain());
        }

        [Fact]
        public void GetAllByAssetIdWitNonExistsIdReturnsEmptyList()
        {
            QueryResponse response = FakeDataHelper.MockQueryResponse<Charge>(0);

            _amazonDynamoDb.Setup(_ => _.QueryAsync(It.IsAny<QueryRequest>(), CancellationToken.None))
                .ReturnsAsync(response);

            Func<Task<List<Charge>>> func = async () => await _sut.GetAllByAssetId(Guid.NewGuid()).ConfigureAwait(false);
            var result = func.Invoke();
            result.Should().NotBeNull();
            result.Result.Count.Should().Be(0);
        }*/

    }
}
