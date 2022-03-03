using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using AutoFixture;
using FinanceServicesApi.Tests.V1.Helper;
using FinanceServicesApi.V1.Boundary.Request;
using FinanceServicesApi.V1.Domain.TransactionModels;
using FinanceServicesApi.V1.Gateways;
using FinanceServicesApi.V1.Infrastructure;
using FinanceServicesApi.V1.Infrastructure.Entities;
using FinanceServicesApi.V1.Infrastructure.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace FinanceServicesApi.Tests.V1.Gateways
{
    public class TransactionGatewayTests
    {
        private readonly Mock<IDynamoDBContext> _dynamoDbContext;
        private readonly Mock<IAmazonDynamoDB> _amazonDynamoDb;
        private readonly Mock<IFinanceDomainApiData<Transaction>> _housingSingleRecord;
        private readonly Mock<IFinanceDomainApiData<List<Transaction>>> _housingMultipleRecords;
        private TransactionGateway _sutGateway;
        private readonly Fixture _fixture;

        public TransactionGatewayTests()
        {
            _fixture = new Fixture();
            _dynamoDbContext = new Mock<IDynamoDBContext>();
            _amazonDynamoDb = new Mock<IAmazonDynamoDB>();
            _housingSingleRecord = new Mock<IFinanceDomainApiData<Transaction>>();
            _housingMultipleRecords = new Mock<IFinanceDomainApiData<List<Transaction>>>();
            _sutGateway = new TransactionGateway(_amazonDynamoDb.Object,
                _dynamoDbContext.Object,
                _housingSingleRecord.Object,
                _housingMultipleRecords.Object);
        }

        [Fact]
        public void GetByIdWithEmptyIdThrowsArgumentException()
        {
            Func<Task<Transaction>> func = async () => await _sutGateway.GetById(Guid.Empty).ConfigureAwait(false);
            func.Should().Throw<ArgumentException>();
        }

        /*[Fact]
        public void GetByIdWithValidIdReturnsData()
        {
            TransactionDbEntity transaction = _fixture.Create<TransactionDbEntity>();

            _dynamoDbContext.Setup(p => p.LoadAsync<TransactionDbEntity>(It.IsAny<Guid>(), It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(transaction);

            var response = _sutGateway.GetById(Guid.NewGuid());
            response.Should().NotBeNull();
            response.Result.Should().BeEquivalentTo(transaction);
        }

        [Fact]
        public void GetByIdWithNonExistsIdThrowsException()
        {
            _dynamoDbContext.Setup(p => p.LoadAsync<TransactionDbEntity>(It.IsAny<Guid>(), It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync((TransactionDbEntity) null);

            Func<Task<Transaction>> func = async () => await _sutGateway.GetById(Guid.NewGuid()).ConfigureAwait(false);

            var exception = func.Should().ThrowAsync<Exception>();
            exception.Result.WithMessage("The transaction api is not reachable!");
        }

        [Fact]
        public async Task GetByTargetIdWithValidRequestReturnsData()
        {
            TransactionsRequest transactionsRequest = _fixture.Create<TransactionsRequest>();
            QueryResponse queryResponse = FakeDataHelper.MockQueryResponse<Transaction>();

            _amazonDynamoDb.Setup(p => p.QueryAsync(It.IsAny<QueryRequest>(), CancellationToken.None))
                .ReturnsAsync(queryResponse);

            var response = await _sutGateway.GetByTargetId(transactionsRequest).ConfigureAwait(false);
            response.Should().NotBeNull();

            queryResponse.ToTransactions().Should().BeEquivalentTo(response);
        }

        [Fact]
        public async Task GetByTargetIdWithNonExistsTargetIdReturnsEmptyTransactionList()
        {
            TransactionsRequest transactionsRequest = _fixture.Create<TransactionsRequest>();
            QueryResponse queryResponse = new QueryResponse();
            _amazonDynamoDb.Setup(p => p.QueryAsync(It.IsAny<QueryRequest>(), CancellationToken.None))
                .ReturnsAsync(queryResponse);

            var response = await _sutGateway.GetByTargetId(transactionsRequest).ConfigureAwait(false);
            response.Should().NotBeNull();
            response.Count.Should().Be(0);
        }*/

        [Fact]
        public void GetByTargetIdWithEmptyTargetIdThrowsArgumentException()
        {
            TransactionsRequest transactionsRequest = _fixture.Build<TransactionsRequest>()
                .With(p => p.TargetId, Guid.Empty)
                .Create();

            _amazonDynamoDb.Verify(p => p.QueryAsync(It.IsAny<QueryRequest>(), CancellationToken.None), Times.Never);

            Func<Task<List<Transaction>>> func = async () => await _sutGateway.GetByTargetId(transactionsRequest).ConfigureAwait(false);
            var exception = func.Should().ThrowAsync<ArgumentException>();
            exception.WithMessage("transactionsRequest shouldn't be empty.");
        }

        [Fact]
        public void GetAllByAssetIdWitNullResponseFromAmazonDynamoDbReturnsNull()
        {
            // Arrange
            TransactionsRequest transactionsRequest = _fixture.Create<TransactionsRequest>();
            _amazonDynamoDb.Setup(_ => _.QueryAsync(It.IsAny<QueryRequest>(), CancellationToken.None))
                .ReturnsAsync((QueryResponse) null);

            // Act
            Func<Task<List<Transaction>>> func = async () => await _sutGateway.GetByTargetId(transactionsRequest).ConfigureAwait(false);
            var result = func.Invoke();

            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeNull();
        }
    }
}
