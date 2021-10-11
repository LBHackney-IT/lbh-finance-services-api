using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using BaseApi.V1.Boundary;
using FluentAssertions;
using Xunit;

namespace BaseApi.Tests.V1.Boundary
{
    public class HealthCheckResponseTests
    {
        private readonly Fixture _fixture;

        public HealthCheckResponseTests()
        {
            _fixture = new Fixture();
        }
        [Fact]
        public void EntityHasProperties()
        {
            var entityType = typeof(HealthCheckResponse);
            entityType.GetProperties().Length.Should().Be(2);

            HealthCheckResponse healthCheck = _fixture.Create<HealthCheckResponse>();
            Assert.IsType<string>(healthCheck.Message);
            Assert.IsType<bool>(healthCheck.Success);
        }
    }
}
