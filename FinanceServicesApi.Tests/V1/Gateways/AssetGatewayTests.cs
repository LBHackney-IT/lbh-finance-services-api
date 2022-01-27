using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AutoFixture;
using FinanceServicesApi.V1.Gateways;
using FinanceServicesApi.V1.Infrastructure.Interfaces;
using FluentAssertions;
using Hackney.Shared.Asset.Domain;
using Moq;
using Xunit;

namespace FinanceServicesApi.Tests.V1.Gateways
{
    [ExcludeFromCodeCoverage]
    public class AssetGatewayTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<IHousingData<Asset>> _housingData;
        private readonly AssetGateway _sutGateway;


        public AssetGatewayTests()
        {
            _fixture = new Fixture();
            _housingData = new Mock<IHousingData<Asset>>();
            _sutGateway = new AssetGateway(_housingData.Object);
        }

        [Fact]
        public void GetByIdWithValidIdReturnsValidData()
        {
            Asset assetResponse = _fixture.Create<Asset>();
            _housingData.Setup(p => p.DownloadAsync(It.IsAny<Guid>()))
                .ReturnsAsync(assetResponse);

            var response = _sutGateway.GetById(Guid.NewGuid());
            response.Should().NotBeNull();
            response.Result.Should().BeEquivalentTo(assetResponse);
        }

        [Fact]
        public async Task GetByIdWithEmptyIdThrowsArgumentException()
        {
            async Task<Asset> Func() => await _sutGateway.GetById(Guid.Empty).ConfigureAwait(false);
            ArgumentException exception =
                await Assert.ThrowsAsync<ArgumentException>((Func<Task<Asset>>) Func).ConfigureAwait(false);
            exception.Message.Should().Be("id shouldn't be empty.");
        }
    }
}
