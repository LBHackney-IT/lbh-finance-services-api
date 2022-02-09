using AutoFixture;
using FinanceServicesApi.V1.Domain.Charges;
using FinanceServicesApi.V1.Gateways;
using FinanceServicesApi.V1.Infrastructure.Interfaces;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace FinanceServicesApi.Tests.V1.Gateways
{
    [ExcludeFromCodeCoverage]
    public class ChargesGatewayTests
    {
        private readonly Mock<IHousingData<List<Charge>>> _housingData;
        private readonly Fixture _fixture;
        private ChargesGateway _sut;

        public ChargesGatewayTests()
        {
            _fixture = new Fixture();
            _housingData = new Mock<IHousingData<List<Charge>>>();
            _sut = new ChargesGateway(_housingData.Object);
        }

        [Fact]
        public void GetAllByAssetIdWithEmptyInputReturnsException()
        {
            Func<Task<List<Charge>>> func = async () => await _sut.GetAllByAssetId(Guid.Empty).ConfigureAwait(false);
            func.Should().Throw<ArgumentException>();
        }

        [Fact]
        public async Task GetAllByAssetIdWitValidInputReturnsData()
        {
            var expectedResponse = _fixture.Create<List<Charge>>();

            _housingData.Setup(_ => _.DownloadAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedResponse);

            Func<Task<List<Charge>>> func = async () => await _sut.GetAllByAssetId(Guid.NewGuid()).ConfigureAwait(false);

            var actualResponse = await func.Invoke().ConfigureAwait(false);
            actualResponse.Should().NotBeNull();
            actualResponse.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task GetAllByAssetIdWitNonExistsIdReturnsEmptyList()
        {
            var expectedResponse = new List<Charge>(0);

            _housingData.Setup(_ => _.DownloadAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedResponse);

            Func<Task<List<Charge>>> func = async () => await _sut.GetAllByAssetId(Guid.NewGuid()).ConfigureAwait(false);

            var actualResponse = await func.Invoke().ConfigureAwait(false);
            actualResponse.Should().NotBeNull();
            actualResponse.Should().BeEquivalentTo(expectedResponse);
        }


        [Fact]
        public void GetAllByAssetIdWitNullResponseFromAmazonDynamoDbReturnsNull()
        {
            // Arrange
            _housingData.Setup(_ => _.DownloadAsync(It.IsAny<Guid>()))
                .ReturnsAsync((List<Charge>) null);

            // Act
            Func<Task<List<Charge>>> func = async () => await _sut.GetAllByAssetId(Guid.NewGuid()).ConfigureAwait(false);
            var result = func.Invoke();

            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeNull();
        }
    }
}
