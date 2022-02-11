using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AutoFixture;
using FinanceServicesApi.V1.Gateways;
using FinanceServicesApi.V1.Infrastructure.Enums;
using FinanceServicesApi.V1.Infrastructure.Interfaces;
using FluentAssertions;
using Hackney.Shared.Person;
using Moq;
using Xunit;

namespace FinanceServicesApi.Tests.V1.Gateways
{
    [ExcludeFromCodeCoverage]
    public class PersonGatewayTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<IHousingData<Person>> _housingData;
        private readonly PersonGateway _sutGateway;


        public PersonGatewayTests()
        {
            _fixture = new Fixture();
            _housingData = new Mock<IHousingData<Person>>();
            _sutGateway = new PersonGateway(_housingData.Object);
        }

        [Fact]
        public void GetByIdWithValidIdReturnsValidData()
        {
            Person chargeResponse = _fixture.Create<Person>();
            _housingData.Setup(p => p.DownloadAsync(It.IsAny<Guid>(), It.IsAny<SearchBy>()))
                .ReturnsAsync(chargeResponse);

            var response = _sutGateway.GetById(Guid.NewGuid());
            response.Should().NotBeNull();
            response.Result.Should().BeEquivalentTo(chargeResponse);
        }

        [Fact]
        public async Task GetByIdWithEmptyIdThrowsArgumentException()
        {
            async Task<Person> Func() => await _sutGateway.GetById(Guid.Empty).ConfigureAwait(false);
            ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>((Func<Task<Person>>) Func).ConfigureAwait(false);
            exception.Message.Should().Be("id shouldn't be empty.");
        }
    }
}
