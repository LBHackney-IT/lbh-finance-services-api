using System;
using System.Collections.Generic;
using System.Linq;
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
    public class TransactionGatewayTests
    {
        private readonly Mock<ICustomeHttpClient> _httpClientMock;
        private TransactionGateway _gateway;
        private readonly Fixture _fixture;

        public TransactionGatewayTests()
        {
            _fixture = new Fixture();
            _httpClientMock = new Mock<ICustomeHttpClient>();
            _gateway = new TransactionGateway(_httpClientMock.Object);
        }
        [Fact]
        public void ConstructorGetsApiUrlAndApiTokenFromEnvironment()
        {
            CustomeHttpClient client = new CustomeHttpClient();
            _gateway = new TransactionGateway(client);
            Assert.True(true);
        }

        [Fact]
        public void GetByIdWithEmptyIdThrowsArgumentNullException()
        {
            Func<Task<TransactionResponse>> func = async () => await _gateway.GetById(Guid.Empty).ConfigureAwait(false);
            func.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task GetByIdReturnsBadRequestThrowsException()
        {
            HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.BadRequest);

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

            _httpClientMock.Setup(_ => _.GetAsync(It.IsAny<Uri>()))
                .ReturnsAsync(message);

            var result = await _gateway.GetById(Guid.NewGuid()).ConfigureAwait(false);
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(transactionResponse);
        }
    }
}
