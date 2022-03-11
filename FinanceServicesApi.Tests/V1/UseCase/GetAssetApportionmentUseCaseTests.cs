using AutoFixture;
using FinanceServicesApi.Tests.V1.Helper;
using FinanceServicesApi.V1.Boundary.Request.Enums;
using FinanceServicesApi.V1.Boundary.Responses.PropertySummary;
using FinanceServicesApi.V1.Domain.Charges;
using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.Infrastructure.Enums;
using FinanceServicesApi.V1.UseCase;
using FinanceServicesApi.V1.UseCase.Interfaces;
using FluentAssertions;
using Hackney.Shared.Asset.Domain;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FinanceServicesApi.Tests.V1.UseCase
{
    public class GetAssetApportionmentUseCaseTests
    {
        private Mock<IGetChargeByAssetIdUseCase> _chargeUseCase;
        private Mock<IAssetGateway> _assetGateway;
        private Fixture _fixture;

        private GetAssetApportionmentUseCase _sut { get; set; }

        public GetAssetApportionmentUseCaseTests()
        {
            _chargeUseCase = new Mock<IGetChargeByAssetIdUseCase>();
            _assetGateway = new Mock<IAssetGateway>();
            _fixture = new Fixture();

            _sut = new GetAssetApportionmentUseCase(_chargeUseCase.Object, _assetGateway.Object);
        }

        [Fact]
        public async Task ChargesGatewayThrowsShouldRethrow()
        {
            var expectedException = new Exception("Some exception message");
            _chargeUseCase.Setup(_ => _.ExecuteAsync(It.IsAny<Guid>()))
                .ThrowsAsync(expectedException);
            _assetGateway.Setup(_ => _.GetById(It.IsAny<Guid>()))
                    .ReturnsAsync(_fixture.Create<Asset>());

            Func<Task> action = () => _sut.ExecuteAsync(Guid.NewGuid(), 2022, ChargeGroupFilter.Both);

            var actualException = await Assert.ThrowsAsync<Exception>(action).ConfigureAwait(false);
            actualException.Should().BeEquivalentTo(expectedException);
        }

        [Fact]
        public async Task ChargesGatewayReturnsNullShouldThrow()
        {
            Guid assetId = Guid.NewGuid();
            var expectedException = new ArgumentException($"No charges was loaded from Charges API for asset id: [{assetId}]");
            _chargeUseCase.Setup(_ => _.ExecuteAsync(It.IsAny<Guid>()))
                .ReturnsAsync((List<Charge>) null);
            _assetGateway.Setup(_ => _.GetById(It.IsAny<Guid>()))
                    .ReturnsAsync(_fixture.Create<Asset>());

            Func<Task> action = () => _sut.ExecuteAsync(assetId, 2022, ChargeGroupFilter.Both);

            var actualException = await Assert.ThrowsAsync<ArgumentException>(action).ConfigureAwait(false);
            actualException.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task ChargesGatewayReturnsZeroItemsShouldThrow()
        {
            Guid assetId = Guid.NewGuid();
            var expectedException = new ArgumentException($"No charges was loaded from Charges API for asset id: [{assetId}]");
            _chargeUseCase.Setup(_ => _.ExecuteAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new List<Charge>());
            _assetGateway.Setup(_ => _.GetById(It.IsAny<Guid>()))
                    .ReturnsAsync(_fixture.Create<Asset>());

            Func<Task> action = () => _sut.ExecuteAsync(assetId, 2022, ChargeGroupFilter.Both);

            var actualException = await Assert.ThrowsAsync<ArgumentException>(action).ConfigureAwait(false);
            actualException.Message.Should().Be(expectedException.Message);
        }

        [Fact]
        public async Task GatewatReturnsLeaseholdChargesCalculateApportionmentFrom2019()
        {
            Guid assetId = Guid.NewGuid();
            Asset asset = _fixture.Create<Asset>();
            var charges = ApportionmentGeneratorHelper.CreateTestChargesData(ChargeGroupFilter.Leaseholders);

            AssetApportionmentResponse expectedResponse = new AssetApportionmentResponse()
            {
                AssetId = assetId,
                AssetAddress = asset.AssetAddress,
                LeaseholdTotals = ApportionmentGeneratorHelper.CalculateDefaultApportionmentTotals(),
                LeaseholdApportionment = ApportionmentGeneratorHelper.CalculateDefaultApportionment(),
                TenantTotals = null,
                TenantApportionment = null
            };
            _chargeUseCase.Setup(_ => _.ExecuteAsync(assetId))
                .ReturnsAsync(charges);
            _assetGateway.Setup(_ => _.GetById(It.IsAny<Guid>()))
                    .ReturnsAsync(asset);

            var actualResponse = await _sut.ExecuteAsync(assetId, (short) (DateTime.Now.Year - 3), ChargeGroupFilter.Leaseholders).ConfigureAwait(false);

            actualResponse.Should().NotBeNull();
            actualResponse.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task GatewatReturnsTenantsChargesCalculateApportionmentFrom2019()
        {
            Guid assetId = Guid.NewGuid();
            Asset asset = _fixture.Create<Asset>();
            var charges = ApportionmentGeneratorHelper.CreateTestChargesData(ChargeGroupFilter.Tenants);

            AssetApportionmentResponse expectedResponse = new AssetApportionmentResponse()
            {
                AssetId = assetId,
                AssetAddress = asset.AssetAddress,
                LeaseholdTotals = null,
                LeaseholdApportionment = null,
                TenantTotals = ApportionmentGeneratorHelper.CalculateDefaultApportionmentTotals(),
                TenantApportionment = ApportionmentGeneratorHelper.CalculateDefaultApportionment()
            };
            _chargeUseCase.Setup(_ => _.ExecuteAsync(assetId))
                .ReturnsAsync(charges);
            _assetGateway.Setup(_ => _.GetById(It.IsAny<Guid>()))
                    .ReturnsAsync(asset);

            var actualResponse = await _sut.ExecuteAsync(assetId, (short) (DateTime.Now.Year - 3), ChargeGroupFilter.Tenants).ConfigureAwait(false);

            actualResponse.Should().NotBeNull();
            actualResponse.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task GatewatReturnsChargesCalculateApportionmentFrom2019()
        {
            Guid assetId = Guid.NewGuid();
            Asset asset = _fixture.Create<Asset>();
            var charges = ApportionmentGeneratorHelper.CreateTestChargesData(ChargeGroupFilter.Both);

            AssetApportionmentResponse expectedResponse = new AssetApportionmentResponse()
            {
                AssetId = assetId,
                AssetAddress = asset.AssetAddress,
                LeaseholdTotals = ApportionmentGeneratorHelper.CalculateDefaultApportionmentTotals(),
                LeaseholdApportionment = ApportionmentGeneratorHelper.CalculateDefaultApportionment(),
                TenantTotals = ApportionmentGeneratorHelper.CalculateDefaultApportionmentTotals(),
                TenantApportionment = ApportionmentGeneratorHelper.CalculateDefaultApportionment()
            };
            _chargeUseCase.Setup(_ => _.ExecuteAsync(assetId))
                .ReturnsAsync(charges);
            _assetGateway.Setup(_ => _.GetById(It.IsAny<Guid>()))
                    .ReturnsAsync(asset);

            var actualResponse = await _sut.ExecuteAsync(assetId, (short) (DateTime.Now.Year - 3), ChargeGroupFilter.Both).ConfigureAwait(false);

            actualResponse.Should().NotBeNull();
            actualResponse.Should().BeEquivalentTo(expectedResponse);
        }
    }
}
