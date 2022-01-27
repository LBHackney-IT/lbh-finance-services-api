using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AutoFixture;
using FinanceServicesApi.V1.Boundary.Responses;
using FinanceServicesApi.V1.Gateways;
using FinanceServicesApi.V1.Infrastructure.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace FinanceServicesApi.Tests.V1.Gateways
{
    [ExcludeFromCodeCoverage]
    public class ContactDetailsGatewayTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<IHousingData<GetContactDetailsResponse>> _housingData;
        private readonly ContactDetailsGateway _sutGateway;


        public ContactDetailsGatewayTests()
        {
            _fixture = new Fixture();
            _housingData = new Mock<IHousingData<GetContactDetailsResponse>>();
            _sutGateway = new ContactDetailsGateway(_housingData.Object);
        }

        [Fact]
        public void GetByIdWithValidIdReturnsValidData()
        {
            GetContactDetailsResponse contactDetailsResponse = _fixture.Create<GetContactDetailsResponse>();
            _housingData.Setup(p => p.DownloadAsync(It.IsAny<Guid>()))
                .ReturnsAsync(contactDetailsResponse);

            var response = _sutGateway.GetByTargetId(Guid.NewGuid());
            response.Should().NotBeNull();
            response.Result.Should().BeEquivalentTo(contactDetailsResponse);
        }

        [Fact]
        public async Task GetByIdWithEmptyIdThrowsArgumentException()
        {
            async Task<GetContactDetailsResponse> Func() => await _sutGateway.GetByTargetId(Guid.Empty).ConfigureAwait(false);
            ArgumentException exception =
                await Assert.ThrowsAsync<ArgumentException>((Func<Task<GetContactDetailsResponse>>) Func).ConfigureAwait(false);
            exception.Message.Should().Be("targetId shouldn't be empty.");
        }
    }
}
