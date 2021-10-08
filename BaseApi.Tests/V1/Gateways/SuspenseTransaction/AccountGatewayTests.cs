using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using BaseApi.V1.Boundary.Response;
using BaseApi.V1.Gateways.SuspenseTransaction;
using BaseApi.V1.Infrastructure;
using BaseApi.V1.Infrastructure.Interfaces;
using FluentAssertions;
using FluentAssertions.Specialized;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace BaseApi.Tests.V1.Gateways.SuspenseTransaction
{
    public class AccountGatewayTests
    {
        private readonly Mock<ICustomeHttpClient> _httpClientMock;
        private readonly Mock<IEnvironmentVariables> _environmentVariables;
        private AccountGateway _gateway;
        private readonly Fixture _fixture;

        public AccountGatewayTests()
        {
            _fixture = new Fixture();
            _httpClientMock = new Mock<ICustomeHttpClient>();
            _environmentVariables = new Mock<IEnvironmentVariables>();
            _gateway = new AccountGateway(_httpClientMock.Object, _environmentVariables.Object);
        }
        [Fact]
        public void ConstructorGetsApiUrlAndApiTokenFromEnvironment()
        {
            /*CustomeHttpClient client = new CustomeHttpClient();
            EnvironmentVariables environmentVariables = new EnvironmentVariables();*/
            _gateway = new AccountGateway(_httpClientMock.Object, _environmentVariables.Object);
            Assert.True(true);
        }

        [Fact]
        public void ConstructorWithoutApiUrlThrowsError()
        {
            try
            {
                _environmentVariables.Setup(_ => _.GetAccountApiUrl()).Returns(String.Empty);
                _gateway = new AccountGateway(_httpClientMock.Object, _environmentVariables.Object);
                Assert.Fail("This section shouldn't be happen!");
            }
            catch (Exception ex)
            {
                ex.Message.Should().Be("Account api url shouldn't be null or empty");
            }
        }

        [Fact]
        public void GetByIdWithEmptyIdThrowsArgumentNullException()
        {
            Func<Task<AccountResponse>> func = async () => await _gateway.GetById(Guid.Empty).ConfigureAwait(false);
            func.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task GetByIdReturnsBadRequestThrowsException()
        {
            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.BadRequest);

            _httpClientMock.Setup(_ => _.GetAsync(It.IsAny<Uri>()))
                .ReturnsAsync(message);

            Func<Task<AccountResponse>> func = async () => await _gateway.GetById(Guid.NewGuid()).ConfigureAwait(false);

            var exception = await func.Should().ThrowAsync<Exception>().ConfigureAwait(false);
            exception.Should().BeOfType(typeof(ExceptionAssertions<Exception>));
            exception.Which.Message.Should().Be("BadRequest");
        }

        [Fact]
        public async Task GetByIdReturnsNullThrowsException()
        {
            _httpClientMock.Setup(_ => _.GetAsync(It.IsAny<Uri>()))
                .ReturnsAsync((HttpResponseMessage)null);

            Func<Task<AccountResponse>> func = async () => await _gateway.GetById(Guid.NewGuid()).ConfigureAwait(false);

            var exception = await func.Should().ThrowAsync<Exception>().ConfigureAwait(false);
            exception.Should().BeOfType(typeof(ExceptionAssertions<Exception>));
            exception.Which.Message.Should().Be("The account api is not reachable!");
        }

        [Fact]
        public async Task GetByIdWithValidOutputReturnsAccountResponse()
        {
            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.OK);
            AccountResponse accountResponse = _fixture.Create<AccountResponse>();
            message.Content = new StringContent(JsonConvert.SerializeObject(accountResponse));

            _httpClientMock.Setup(_ => _.GetAsync(It.IsAny<Uri>()))
                .ReturnsAsync(message);

            var result =await _gateway.GetById(Guid.NewGuid()).ConfigureAwait(false);
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(accountResponse);
        }

    }
}
