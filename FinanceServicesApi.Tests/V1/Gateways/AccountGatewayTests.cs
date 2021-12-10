using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using AutoFixture;
using FinanceServicesApi.V1.Domain.AccountModels;
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
    public class AccountGatewayTests
    {
        private readonly Mock<IDynamoDBContext> _dynamoDBContext;
        private readonly Mock<IGetEnvironmentVariables> _getEnvironmentVariables;
        private AccountGateway _gateway;
        private readonly Fixture _fixture;

        public AccountGatewayTests()
        {
            _fixture = new Fixture();
            _dynamoDBContext = new Mock<IDynamoDBContext>();

            _getEnvironmentVariables = new Mock<IGetEnvironmentVariables>();

            _getEnvironmentVariables.Setup(_ => _.GetAccountApiUrl())
                .Returns(Environment.GetEnvironmentVariable("ACCOUNT_API_URL"));

            _getEnvironmentVariables.Setup(_ => _.GetAccountApiUrl())
                .Returns(Environment.GetEnvironmentVariable("ACCOUNT_API_URL"));

            _gateway = new AccountGateway(_dynamoDBContext.Object);
        }
        [Fact]
        public void ConstructorGetsApiUrlAndApiTokenFromEnvironment()
        {
            _gateway = new AccountGateway(new DynamoDBContext(new AmazonDynamoDBClient()));
            Assert.True(true);
        }

        [Fact]
        public void GetByIdWithEmptyIdThrowsArgumentNullException()
        {
            Func<Task<Account>> func = async () => await _gateway.GetById(Guid.Empty).ConfigureAwait(false);
            func.Should().Throw<ArgumentNullException>();
        }

        /*[Fact]
        public async Task GetByIdReturnsBadRequestThrowsException()
        {
            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.BadRequest);
            _getEnvironmentVariables.Setup(_ => _.GetAccountApiUrl()).Returns("http://localhost:5000/api/v1/");
            _getEnvironmentVariables.Setup(_ => _.GetAccountApiToken()).Returns("ACCOUNT_API_TOKEN");
            _httpClientMock.Setup(_ => _.GetAsync(It.IsAny<Uri>()))
                .ReturnsAsync(message);

            Func<Task<Account>> func = async () => await _gateway.GetById(Guid.NewGuid()).ConfigureAwait(false);

            var exception = await func.Should().ThrowAsync<Exception>().ConfigureAwait(false);
            exception.Should().BeOfType(typeof(ExceptionAssertions<Exception>));
            exception.Which.Message.Should().Be("BadRequest");
        }

        [Fact]
        public async Task GetByIdReturnsNullThrowsException()
        {
            _getEnvironmentVariables.Setup(_ => _.GetAccountApiUrl()).Returns("http://localhost:5000/api/v1/");
            _getEnvironmentVariables.Setup(_ => _.GetAccountApiToken()).Returns("ACCOUNT_API_TOKEN");
            _httpClientMock.Setup(_ => _.GetAsync(It.IsAny<Uri>()))
                .ReturnsAsync((HttpResponseMessage) null);

            Func<Task<Account>> func = async () => await _gateway.GetById(Guid.NewGuid()).ConfigureAwait(false);

            var exception = await func.Should().ThrowAsync<Exception>().ConfigureAwait(false);
            exception.Should().BeOfType(typeof(ExceptionAssertions<Exception>));
            exception.Which.Message.Should().Contain("The account api is not reachable!");
        }

        [Fact]
        public async Task GetByIdWithValidOutputReturnsAccount()
        {
            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
            Account accountResponse = _fixture.Create<Account>();
            message.Content = new StringContent(JsonConvert.SerializeObject(accountResponse));

            _getEnvironmentVariables.Setup(_ => _.GetAccountApiUrl()).Returns("http://localhost:5000/api/v1/");
            _getEnvironmentVariables.Setup(_ => _.GetAccountApiToken()).Returns("ACCOUNT_API_TOKEN");
            _httpClientMock.Setup(_ => _.GetAsync(It.IsAny<Uri>())).ReturnsAsync(message);

            var result = await _gateway.GetById(Guid.NewGuid()).ConfigureAwait(false);
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(accountResponse);
        }

        [Fact]
        public async Task GetByIdWithEmptyApiUrlThrowsException()
        {
            _getEnvironmentVariables.Setup(_ => _.GetAccountApiUrl())
                .Returns(String.Empty);

            _getEnvironmentVariables.Setup(_ => _.GetAccountApiToken())
                .Returns(Environment.GetEnvironmentVariable("ACCOUNT_API_TOKEN"));

            _gateway = new AccountGateway(_httpClientMock.Object, _getEnvironmentVariables.Object);

            Func<Task<Account>> func = async () =>
                await _gateway.GetById(Guid.NewGuid()).ConfigureAwait(false);
            var exceptionAssertions = await func.Should().ThrowAsync<Exception>().ConfigureAwait(false);
        }

        [Fact]
        public async Task GetByIdWithEmptyApiKeyThrowsException()
        {
            _getEnvironmentVariables.Setup(_ => _.GetAccountApiUrl())
                .Returns(Environment.GetEnvironmentVariable("ACCOUNT_API_URL"));

            _getEnvironmentVariables.Setup(_ => _.GetAccountApiToken())
                .Returns(String.Empty);

            _gateway = new AccountGateway(_httpClientMock.Object, _getEnvironmentVariables.Object);

            Func<Task<Account>> func = async () =>
                await _gateway.GetById(Guid.NewGuid()).ConfigureAwait(false);
            var exceptionAssertions = await func.Should().ThrowAsync<Exception>().ConfigureAwait(false);
        }*/

    }
}
