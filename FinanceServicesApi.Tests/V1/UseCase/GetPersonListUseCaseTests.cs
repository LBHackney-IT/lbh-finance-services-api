using AutoFixture;
using FinanceServicesApi.V1.Boundary.Request;
using FinanceServicesApi.V1.Boundary.Responses;
using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.UseCase;
using FinanceServicesApi.V1.UseCase.Interfaces;
using FluentAssertions;
using Hackney.Shared.HousingSearch.Domain.Person;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace FinanceServicesApi.Tests.V1.UseCase
{
    public class GetPersonListUseCaseTests
    {
        private readonly Mock<IHousingSearchGateway> _housingSearchGateway;
        private readonly Mock<IGetAccountByTargetIdUseCase> _getAccountByTargetIdUseCase;
        private readonly Fixture _fixture;

        private readonly GetPersonListUseCase _sut;

        public GetPersonListUseCaseTests()
        {
            _fixture = new Fixture();

            _housingSearchGateway = new Mock<IHousingSearchGateway>();
            _getAccountByTargetIdUseCase = new Mock<IGetAccountByTargetIdUseCase>();

            _sut = new GetPersonListUseCase(_housingSearchGateway.Object, _getAccountByTargetIdUseCase.Object);
        }

        [Fact]
        public async Task PersonGatewayReturnsEmptyListReturnsEmptyList()
        {
            _housingSearchGateway.Setup(_ => _.GetPersons(It.IsAny<GetPersonListRequest>()))
                .ReturnsAsync(new GetPersonListResponse() { Persons = new List<Person>() });

            var actualResponse = await _sut.ExecuteAsync(_fixture.Create<GetPersonListRequest>()).ConfigureAwait(false);

            var expectedResponse = new GetPersonListResponse()
            {
                Persons = new List<Person>()
            };

            actualResponse.Should().NotBeNull();
            actualResponse.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task PersonGatewayReturnsNullListReturnsEmptyList()
        {
            _housingSearchGateway.Setup(_ => _.GetPersons(It.IsAny<GetPersonListRequest>()))
                .ReturnsAsync(new GetPersonListResponse());

            var actualResponse = await _sut.ExecuteAsync(_fixture.Create<GetPersonListRequest>()).ConfigureAwait(false);

            var expectedResponse = new GetPersonListResponse();

            actualResponse.Should().NotBeNull();
            actualResponse.Should().BeEquivalentTo(expectedResponse);
        }
    }
}
