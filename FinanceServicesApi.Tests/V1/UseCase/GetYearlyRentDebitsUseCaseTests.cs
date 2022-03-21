using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.UseCase;
using Moq;
using System;
using FinanceServicesApi.Tests.Extensions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace FinanceServicesApi.Tests.V1.UseCase
{
    public class GetYearlyRentDebitsUseCaseTests
    {
        private readonly Bogus.Faker _faker;
        private readonly GetYearlyRentDebitsUseCase _useCase;
        private readonly Mock<IAssetGateway> _assetGateway;
        private readonly Mock<IChargesGateway> _chargesGateway;
        private readonly Mock<ITenureInformationGateway> _tenureInformationGateway;

        public GetYearlyRentDebitsUseCaseTests()
        {
            _faker = new Bogus.Faker();
            _assetGateway = new Mock<IAssetGateway>();
            _chargesGateway = new Mock<IChargesGateway>();
            _tenureInformationGateway = new Mock<ITenureInformationGateway>();
            _useCase = new GetYearlyRentDebitsUseCase(_assetGateway.Object, _chargesGateway.Object,
                _tenureInformationGateway.Object);
        }

        [Fact]
        public void ShouldRaiseExceptionsIfAssetIdIsEmpty()
        {
            var assetid = Guid.Empty;
            _useCase
                .Invoking(useCase => useCase.ExecuteAsync(assetid))
                .Should().Throw<ArgumentException>();
            _assetGateway.VerifyGetAssetById(Times.Never());
            _chargesGateway.VerifyGetChargesByAssetId(Times.Never());
            _tenureInformationGateway.VerifyGetTenureById(Times.Never());
        }
    }
}
