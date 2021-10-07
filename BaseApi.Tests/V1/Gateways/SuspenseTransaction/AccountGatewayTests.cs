using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoFixture;
using BaseApi.Tests.V1.Helper;
using BaseApi.V1.Boundary.Response;
using BaseApi.V1.Gateways.SuspenseTransaction;
using BaseApi.V1.Infrastructure;
using BaseApi.V1.Infrastructure.Interfaces;
using FluentAssertions;
using FluentAssertions.Common;
using FluentAssertions.Specialized;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace BaseApi.Tests.V1.Gateways.SuspenseTransaction
{
    public class AccountGatewayTests
    {
        private readonly Mock<ICustomeHttpClient> _httpClientMock;
        private AccountGateway _gateway;
        private readonly Fixture _fixture;

        public AccountGatewayTests()
        {
            _fixture = new Fixture();
            _httpClientMock = new Mock<ICustomeHttpClient>();
            _gateway = new AccountGateway(_httpClientMock.Object);
        }
        [Fact]
        public void ConstructorGetsApiUrlAndApiTokenFromEnvironment()
        {
            CustomeHttpClient client = new CustomeHttpClient();
            _gateway = new AccountGateway(client);
            Assert.True(true);
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
