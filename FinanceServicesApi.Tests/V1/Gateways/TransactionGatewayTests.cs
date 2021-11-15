using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using FinanceServicesApi.V1.Boundary.Response;
using FinanceServicesApi.V1.Gateways;
using FinanceServicesApi.V1.Infrastructure;
using FinanceServicesApi.V1.Infrastructure.Interfaces;
using FluentAssertions;
using FluentAssertions.Specialized;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace FinanceServicesApi.Tests.V1.Gateways
{
    public class TransactionGatewayTests
    {
        private readonly Mock<ICustomeHttpClient> _httpClientMock;
        private readonly Mock<IGetEnvironmentVariables> _getEnvironmentVariables;
        private TransactionGateway _gateway;
        private readonly Fixture _fixture;

        public TransactionGatewayTests()
        {
            _fixture = new Fixture();
            _httpClientMock = new Mock<ICustomeHttpClient>();
            _getEnvironmentVariables = new Mock<IGetEnvironmentVariables>();

            _getEnvironmentVariables.Setup(_ => _.GetTransactionApiUrl())
                .Returns(Environment.GetEnvironmentVariable("TRANSACTION_API_URL"));

            _getEnvironmentVariables.Setup(_ => _.GetTransactionApiKey())
                .Returns(Environment.GetEnvironmentVariable("TRANSACTION_API_KEY"));

            _gateway = new TransactionGateway(_httpClientMock.Object, _getEnvironmentVariables.Object);
        }
        [Fact]
        public void ConstructorGetsApiUrlAndApiTokenFromEnvironment()
        {
            _gateway = new TransactionGateway(new CustomeHttpClient(), new GetEnvironmentVariables());
            Assert.True(true);
        }

        [Fact]
        public void GetByIdWithEmptyIdThrowsArgumentNullException()
        {
            _gateway = new TransactionGateway(_httpClientMock.Object, _getEnvironmentVariables.Object);
            Func<Task<TransactionResponse>> func = async () => await _gateway.GetById(Guid.Empty).ConfigureAwait(false);
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

            Func<Task<TransactionResponse>> func = async () => await _gateway.GetById(Guid.NewGuid()).ConfigureAwait(false);

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

            Func<Task<TransactionResponse>> func = async () => await _gateway.GetById(Guid.NewGuid()).ConfigureAwait(false);

            var exception = await func.Should().ThrowAsync<Exception>().ConfigureAwait(false);
            exception.Should().BeOfType(typeof(ExceptionAssertions<Exception>));
            exception.Which.Message.Should().Be("The transaction api is not reachable!");
        }

        [Fact]
        public async Task GetByIdWithValidOutputReturnsTransactionResponse()
        {
            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
            TransactionResponse transactionResponse = _fixture.Create<TransactionResponse>();
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

            Func<Task<TransactionResponse>> func = async () =>
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

            Func<Task<TransactionResponse>> func = async () =>
                await _gateway.GetById(Guid.NewGuid()).ConfigureAwait(false);
            var exceptionAssertions = await func.Should().ThrowAsync<Exception>().ConfigureAwait(false);
        }
    }
}
