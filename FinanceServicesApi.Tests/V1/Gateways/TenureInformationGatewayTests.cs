using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AutoFixture;
using FinanceServicesApi.V1.Gateways;
using FinanceServicesApi.V1.Infrastructure.Enums;
using FinanceServicesApi.V1.Infrastructure.Interfaces;
using FluentAssertions;
using Hackney.Shared.Tenure.Domain;
using Moq;
using Xunit;

namespace FinanceServicesApi.Tests.V1.Gateways
{
    [ExcludeFromCodeCoverage]
    public class TenureInformationGatewayTests
    {
        private readonly Fixture _fixture;
        private Mock<IFinanceDomainApiData<TenureInformation>> _housingData;
        private TenureInformationGateway _sut;

        public TenureInformationGatewayTests()
        {
            _fixture = new Fixture();
            _housingData = new Mock<IFinanceDomainApiData<TenureInformation>>();
            _sut = new TenureInformationGateway(_housingData.Object);
        }

        [Fact]
        public void GetByIdWithValidIdReturnsValidData()
        {
            TenureInformation tenureResponse = _fixture.Create<TenureInformation>();
            _housingData.Setup(p => p.DownloadAsync(It.IsAny<Guid>(), It.IsAny<SearchBy>()))
                .ReturnsAsync(tenureResponse);

            var response = _sut.GetById(Guid.NewGuid());
            response.Should().NotBeNull();
            response.Result.Should().BeEquivalentTo(tenureResponse);
        }

        [Fact]
        public async Task GetByIdWithEmptyIdThrowsArgumentException()
        {
            async Task<TenureInformation> Func() => await _sut.GetById(Guid.Empty).ConfigureAwait(false);
            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>((Func<Task<TenureInformation>>) Func).ConfigureAwait(false);
            exception.Message.Should().Be("id shouldn't be empty.");
        }

    }
}
