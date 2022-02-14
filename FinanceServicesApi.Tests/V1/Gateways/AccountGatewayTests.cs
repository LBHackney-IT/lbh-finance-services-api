using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using AutoFixture;
using FinanceServicesApi.Tests.V1.Helper;
using FinanceServicesApi.V1.Domain.AccountModels;
using FinanceServicesApi.V1.Gateways;
using FinanceServicesApi.V1.Infrastructure;
using FinanceServicesApi.V1.Infrastructure.Entities;
using FinanceServicesApi.V1.Infrastructure.Interfaces;
using FluentAssertions;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Boundary.Responses;
using Xunit;

namespace FinanceServicesApi.Tests.V1.Gateways
{
    public class AccountGatewayTests
    {
        private readonly Mock<IDynamoDBContext> _dynamoDbContext;
        private readonly Mock<IAmazonDynamoDB> _amazonDynamoDb;
        private readonly Mock<IHousingData<Account>> _housingData;
        private readonly Mock<IHousingData<GetAccountListResponse>> _housingDataList;
        private readonly Fixture _fixture;
        private AccountGateway _sut;

        public AccountGatewayTests()
        {
            _fixture = new Fixture();
            _dynamoDbContext = new Mock<IDynamoDBContext>();
            _amazonDynamoDb = new Mock<IAmazonDynamoDB>();
            _housingData = new Mock<IHousingData<Account>>();
            _housingDataList = new Mock<IHousingData<GetAccountListResponse>>();
            _sut = new AccountGateway(_dynamoDbContext.Object, _amazonDynamoDb.Object, _housingData.Object, _housingDataList.Object);
        }

        /* [Fact]
         public async Task GetByIdWithExistenceIdReturnsDomain()
         {
             // Arrange
             Guid id = Guid.NewGuid();
             AccountDbEntity account = _fixture.Build<AccountDbEntity>()
                 .With(p => p.Id, id)
                 .Create();

             _dynamoDbContext.Setup(p => p.LoadAsync<AccountDbEntity>(It.IsAny<Guid>(), CancellationToken.None))
                 .ReturnsAsync(account);

             // Act
             var result = await _sut.GetById(id).ConfigureAwait(false);

             //Assert
             result.Should().NotBeNull();
             result.Should().BeEquivalentTo(account);
         }*/

        [Fact]
        public async Task GetByIdWithNonExistenceIdReturnsNull()
        {
            // Arrange
            AccountDbEntity account = _fixture.Build<AccountDbEntity>().Create();

            _dynamoDbContext.Setup(p => p.LoadAsync<AccountDbEntity>(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync((AccountDbEntity) null);

            // Act
            var result = await _sut.GetById(Guid.NewGuid()).ConfigureAwait(false);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetByIdWithEmptyIdThrowsArgumentException()
        {
            // Arrange & Act
            Func<Task<Account>> func = async () => await _sut.GetById(Guid.Empty).ConfigureAwait(false);

            //Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(func);
            exception.Should().NotBeNull();
            exception.Result.Should().BeOfType<ArgumentException>();
            exception.Result.Message.Should().Contain("id shouldn't be empty.");
        }

        [Fact]
        public void GetAllByTargetIdWitExistsIdReturnsAccountDomain()
        {
            // Arrange
            QueryResponse response = FakeDataHelper.MockQueryResponse<Account>(0);

            _amazonDynamoDb.Setup(_ => _.QueryAsync(It.IsAny<QueryRequest>(), CancellationToken.None))
                .ReturnsAsync(response);

            // Act
            Func<Task<Account>> func = async () => await _sut.GetByTargetId(Guid.NewGuid()).ConfigureAwait(false);
            var result = func.Invoke();

            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeEquivalentTo(response.ToAccount());
        }

        [Fact]
        public void GetAllByTargetIdWitNonExistsIdReturnsNull()
        {
            // Arrange
            _amazonDynamoDb.Setup(_ => _.QueryAsync(It.IsAny<QueryRequest>(), CancellationToken.None))
                .ReturnsAsync((QueryResponse) null);

            // Act
            Func<Task<Account>> func = async () => await _sut.GetByTargetId(Guid.NewGuid()).ConfigureAwait(false);
            var response = func.Invoke();

            // Assert
            response.Should().NotBeNull();
            response.Result.Should().BeNull();
        }

    }
}
