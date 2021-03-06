using System.Threading;
using FinanceServicesApi.V1.UseCase;
using FluentAssertions;
using Microsoft.Extensions.HealthChecks;
using Moq;
using Xunit;


namespace FinanceServicesApi.Tests.V1.UseCase
{

    public class DbHealthCheckUseCaseTests
    {

        private Mock<IHealthCheckService> _mockHealthCheckService;
        private DbHealthCheckUseCase _classUnderTest;

        private readonly Bogus.Faker _faker = new Bogus.Faker();
        private string _description;

        public DbHealthCheckUseCaseTests()
        {
            _description = _faker.Random.Words();

            _mockHealthCheckService = new Mock<IHealthCheckService>();
            CompositeHealthCheckResult compositeHealthCheckResult = new CompositeHealthCheckResult(CheckStatus.Healthy);
            compositeHealthCheckResult.Add("test", CheckStatus.Healthy, _description);


            _mockHealthCheckService.Setup(s =>
                    s.CheckHealthAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(compositeHealthCheckResult);

            _classUnderTest = new DbHealthCheckUseCase(_mockHealthCheckService.Object);
        }

        [Fact]
        public void ReturnsResponseWithStatus()
        {
            var response = _classUnderTest.Execute();

            response.Should().NotBeNull();
            response.Success.Should().BeTrue();
            response.Message.Should().BeEquivalentTo("test: " + _description);
        }
    }
}
