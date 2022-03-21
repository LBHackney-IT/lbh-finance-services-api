using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.UseCase;
using Moq;
using System;
using FinanceServicesApi.Tests.Extensions;
using FinanceServicesApi.V1.Infrastructure.Exceptions;
using FluentAssertions;
using Hackney.Shared.Asset.Domain;
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
        public void ShouldRaiseExceptionIfAssetIdIsEmpty()
        {
            var assetId = Guid.Empty;
            _useCase
                .Invoking(useCase => useCase.ExecuteAsync(assetId))
                .Should().Throw<ArgumentException>();
            _assetGateway.VerifyGetAssetById(Times.Never());
            _chargesGateway.VerifyGetChargesByAssetId(Times.Never());
            _tenureInformationGateway.VerifyGetTenureById(Times.Never());
        }

        [Fact]
        public void ShouldRaiseExceptionIfAssetDoesNotExist()
        {
            var assetId = Guid.NewGuid();
            _assetGateway.Setup(g => g.GetById(It.IsAny<Guid>())).ReturnsAsync((Asset)null);
            _useCase
                .Invoking(useCase => useCase.ExecuteAsync(assetId))
                .Should().Throw<AssetNotFoundException>();
            _assetGateway.VerifyGetAssetById(Times.Once());
            _chargesGateway.VerifyGetChargesByAssetId(Times.Never());
            _tenureInformationGateway.VerifyGetTenureById(Times.Never());
        }
    }
}
