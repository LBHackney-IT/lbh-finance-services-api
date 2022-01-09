using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Threading.Tasks;
using AutoFixture;
using FinanceServicesApi.V1.Infrastructure;
using FinanceServicesApi.V1.Infrastructure.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace FinanceServicesApi.Tests.V1.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public abstract class HousingDataTests<T> where
        T : class
    {
        private readonly Fixture _fixture;

        private readonly Mock<ICustomeHttpClient> _client;
        private readonly HttpContext _httpContext = new DefaultHttpContext();
        private readonly Mock<IHttpContextAccessor> _context;

        private readonly Mock<IGenerateUrl<T>> _generateUrl;
        private readonly HousingData<T> _sutHousingData;

        protected HousingDataTests()
        {
            Guid token = Guid.NewGuid();

            _fixture = new Fixture();
            _client = new Mock<ICustomeHttpClient>();
            _generateUrl = new Mock<IGenerateUrl<T>>();
            _context = new Mock<IHttpContextAccessor>();

            _httpContext.Request.Headers["Authorization"] = token.ToString();
            _context.Setup(_ => _.HttpContext).Returns(_httpContext);

            _sutHousingData = new HousingData<T>(_client.Object, _generateUrl.Object, _context.Object);
        }

        public virtual void DownloadAsyncWithEmptyIdThrowsArgumentException()
        {
            // Arrange, Act
            Func<Task<T>> func = async () => await _sutHousingData.DownloadAsync(Guid.Empty).ConfigureAwait(false);

            //Assert
            var exception = func.Should().ThrowAsync<ArgumentException>();
            exception.WithMessage("id shouldn't be empty.");
        }

        public virtual void DownloadAsyncWithoutAuthorizationThrowsInvalidCredentialException()
        {
            //Arrange
            _httpContext.Request.Headers["Authorization"] = string.Empty;

            // Arrange, Act
            Func<Task<T>> func = async () => await _sutHousingData.DownloadAsync(Guid.Empty).ConfigureAwait(false);

            //Assert
            var exception = func.Should().ThrowAsync<InvalidCredentialException>();
            exception.WithMessage("Api token shouldn't be null or empty.");
        }

        public virtual void DownloadAsyncWithExistenceIdReturnsValidData()
        {
            // Arrange
            Guid id = Guid.NewGuid();

            var model = _fixture.Create<T>();
            HttpResponseMessage responseMessage = new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(model))
            };

            _client.Setup(p => p.GetAsync(It.IsAny<Uri>()))
                .ReturnsAsync(responseMessage);

            // Act
            var response = _sutHousingData.DownloadAsync(id);

            //Assert
            response.Should().NotBeNull();
            response.Result.Should().BeOfType<T>();
            response.Result.Should().BeEquivalentTo(model);
        }

        public virtual void DownloadAsyncWithNonReachableUrlThrowsException()
        {
            // Arrange
            Guid id = Guid.NewGuid();

            _client.Setup(p => p.GetAsync(It.IsAny<Uri>()))
                .ReturnsAsync((HttpResponseMessage) null);

            // Act
            Func<Task<T>> func = async () => await _sutHousingData.DownloadAsync(id).ConfigureAwait(false);

            //Assert
            var exception = func.Should().ThrowAsync<Exception>();
            exception.WithMessage($"{nameof(T)} api is not reachable.");
        }

        public virtual void DownloadAsyncWithNonReachableApiThrowsException()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.BadGateway);

            _client.Setup(p => p.GetAsync(It.IsAny<Uri>()))
                .ReturnsAsync(responseMessage);

            // Act
            Func<Task<T>> func = async () => await _sutHousingData.DownloadAsync(id).ConfigureAwait(false);

            //Assert
            var exception = func.Should().ThrowAsync<Exception>();
            exception.WithMessage(HttpStatusCode.BadGateway.ToString());
        }

        public virtual async Task DownloadAsyncWithNonExistenceIdReturnsNull()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);

            _client.Setup(p => p.GetAsync(It.IsAny<Uri>()))
                .ReturnsAsync(responseMessage);

            // Act
            var result = await _sutHousingData.DownloadAsync(id).ConfigureAwait(false);

            //Assert
            result.Should().BeNull();
        }

        public virtual void DownloadAsyncWithApiExceptionReturnsException()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.Unauthorized);

            _client.Setup(p => p.GetAsync(It.IsAny<Uri>()))
                .ReturnsAsync(responseMessage);

            // Act
            Func<Task<T>> func = async () => await _sutHousingData.DownloadAsync(id).ConfigureAwait(false);

            //Assert
            var exception = func.Should().ThrowAsync<Exception>();
            exception.WithMessage(HttpStatusCode.Unauthorized.ToString());
        }
    }
}
