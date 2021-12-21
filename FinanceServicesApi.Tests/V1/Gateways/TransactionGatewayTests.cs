using System;
using System.Transactions;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using AutoFixture;
using FinanceServicesApi.V1.Gateways;
using FinanceServicesApi.V1.Infrastructure.Interfaces;
using Moq;

namespace FinanceServicesApi.Tests.V1.Gateways
{
    public class TransactionGatewayTests
    {
        private readonly Mock<IDynamoDBContext> _dynamoDBContext;
        private readonly Mock<IAmazonDynamoDB> _amazonDynamoDB;
        private readonly Mock<IGetEnvironmentVariables<Transaction>> _getEnvironmentVariables;
        private TransactionGateway _gateway;
        private readonly Fixture _fixture;

        public TransactionGatewayTests()
        {
            _fixture = new Fixture();
            _dynamoDBContext = new Mock<IDynamoDBContext>();
            _amazonDynamoDB = new Mock<IAmazonDynamoDB>();
            _getEnvironmentVariables = new Mock<IGetEnvironmentVariables<Transaction>>();

            _getEnvironmentVariables.Setup(_ => _.GetUrl())
                .Returns(It.IsAny<Uri>());

            _getEnvironmentVariables.Setup(_ => _.GetToken())
                .Returns(Environment.GetEnvironmentVariable("TRANSACTION_API_KEY"));

            _gateway = new TransactionGateway(_amazonDynamoDB.Object, _dynamoDBContext.Object);
        }
        /*[Fact]
        public void ConstructorGetsApiUrlAndApiTokenFromEnvironment()
        {
            _gateway = new TransactionGateway(new CustomeHttpClient(), new GetEnvironmentVariables());
            Assert.True(true);
        }

        [Fact]
        public void GetByIdWithEmptyIdThrowsArgumentNullException()
        {
            _gateway = new TransactionGateway(_httpClientMock.Object, _getEnvironmentVariables.Object);
            Func<Task<Transaction>> func = async () => await _gateway.GetById(Guid.Empty).ConfigureAwait(false);
            func.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task GetByIdReturnsBadRequestThrowsException()
        {
            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.BadRequest);
            _getEnvironmentVariables.Setup(_ => _.GetTransactionApiUrl()).Returns("http://localhost:5000/api/v1/");
            _getEnvironmentVariables.Setup(_ => _.GetTransactionApiKey()).Returns("TRANSACTION_API_KEY");
            _httpClientMock.Setup(_ => _.GetAsync(It.IsAny<Uri>()))
                .ReturnsAsync(message);

            Func<Task<Transaction>> func = async () => await _gateway.GetById(Guid.NewGuid()).ConfigureAwait(false);

            var exception = await func.Should().ThrowAsync<Exception>().ConfigureAwait(false);
            exception.Should().BeOfType(typeof(ExceptionAssertions<Exception>));
            exception.Which.Message.Should().Be("BadRequest");
        }

        [Fact]
        public async Task GetByIdReturnsNullThrowsException()
        {
            _getEnvironmentVariables.Setup(_ => _.GetTransactionApiUrl()).Returns("http://localhost:5000/api/v1/");
            _getEnvironmentVariables.Setup(_ => _.GetTransactionApiKey()).Returns("TRANSACTION_API_KEY");
            _httpClientMock.Setup(_ => _.GetAsync(It.IsAny<Uri>()))
                .ReturnsAsync((HttpResponseMessage) null);

            Func<Task<Transaction>> func = async () => await _gateway.GetById(Guid.NewGuid()).ConfigureAwait(false);

            var exception = await func.Should().ThrowAsync<Exception>().ConfigureAwait(false);
            exception.Should().BeOfType(typeof(ExceptionAssertions<Exception>));
            exception.Which.Message.Should().Be("The transaction api is not reachable!");
        }

        [Fact]
        public async Task GetByIdWithValidOutputReturnsTransaction()
        {
            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
            Transaction transactionResponse = _fixture.Create<Transaction>();
            message.Content = new StringContent(JsonConvert.SerializeObject(transactionResponse));

            _getEnvironmentVariables.Setup(_ => _.GetTransactionApiUrl()).Returns("http://localhost:5000/api/v1/");
            _getEnvironmentVariables.Setup(_ => _.GetTransactionApiKey()).Returns("TRANSACTION_API_KEY");
            _httpClientMock.Setup(_ => _.GetAsync(It.IsAny<Uri>()))
                .ReturnsAsync(message);

            var result = await _gateway.GetById(Guid.NewGuid()).ConfigureAwait(false);
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(transactionResponse);
        }

        [Fact]
        public async Task GetByIdWithEmptyApiUrlThrowsException()
        {
            _getEnvironmentVariables.Setup(_ => _.GetTransactionApiUrl())
                .Returns(String.Empty);

            _getEnvironmentVariables.Setup(_ => _.GetTransactionApiKey())
                .Returns(Environment.GetEnvironmentVariable("TRANSACTION_API_KEY"));

            _gateway = new TransactionGateway(_httpClientMock.Object, _getEnvironmentVariables.Object);

            Func<Task<Transaction>> func = async () =>
                await _gateway.GetById(Guid.NewGuid()).ConfigureAwait(false);
            var exceptionAssertions = await func.Should().ThrowAsync<Exception>().ConfigureAwait(false);
        }

        [Fact]
        public async Task GetByIdWithEmptyApiKeyThrowsException()
        {
            _getEnvironmentVariables.Setup(_ => _.GetTransactionApiUrl())
                .Returns(Environment.GetEnvironmentVariable("TRANSACTION_API_URL"));

            _getEnvironmentVariables.Setup(_ => _.GetTransactionApiKey())
                .Returns(String.Empty);

            _gateway = new TransactionGateway(_httpClientMock.Object, _getEnvironmentVariables.Object);

            Func<Task<Transaction>> func = async () =>
                await _gateway.GetById(Guid.NewGuid()).ConfigureAwait(false);
            var exceptionAssertions = await func.Should().ThrowAsync<Exception>().ConfigureAwait(false);
        }*/
    }
}
