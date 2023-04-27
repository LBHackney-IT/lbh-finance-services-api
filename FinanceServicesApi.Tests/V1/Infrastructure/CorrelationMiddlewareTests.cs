using System.Threading.Tasks;
using FinanceServicesApi.V1;
using FinanceServicesApi.V1.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace FinanceServicesApi.Tests.V1.Infrastructure
{

    public class CorrelationMiddlewareTest
    {
        private readonly CorrelationMiddleware _sut;

        public CorrelationMiddlewareTest()
        {
            _sut = new CorrelationMiddleware(null);
        }

        [Fact]
        public async Task DoesNotReplaceCorrelationIdIfOneExists()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            var headerValue = "123";

            httpContext.HttpContext.Request.Headers.Add(FinanceServicesApiConstants.CorrelationId, headerValue);

            // Act
            await _sut.InvokeAsync(httpContext).ConfigureAwait(false);

            // Assert
            httpContext.HttpContext.Request.Headers[FinanceServicesApiConstants.CorrelationId].Should().BeEquivalentTo(headerValue);
        }

        [Fact]
        public async Task AddsCorrelationIdIfOneDoesNotExist()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();

            // Act
            await _sut.InvokeAsync(httpContext).ConfigureAwait(false);

            // Assert
            httpContext.HttpContext.Request.Headers[FinanceServicesApiConstants.CorrelationId].Should().HaveCountGreaterThan(0);
        }
    }
}
