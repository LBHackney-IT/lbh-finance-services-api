using System.Linq;
using AutoFixture;
using FinanceServicesApi.V1.Boundary.Response;
using FinanceServicesApi.V1.Domain.SuspenseTransaction;
using FinanceServicesApi.V1.Factories;
using FinanceServicesApi.V1.Gateways;
using FinanceServicesApi.V1.UseCase;
using FluentAssertions;
using Moq;
using Xunit;


namespace FinanceServicesApi.Tests.V1.UseCase
{
    public class GetAllUseCaseTests
    {
        /*private Mock<IExampleGateway> _mockGateway;
        private GetAllUseCase _classUnderTest;
        private Fixture _fixture;

        public GetAllUseCaseTests()
        {
            _mockGateway = new Mock<IExampleGateway>();
            _classUnderTest = new GetAllUseCase(_mockGateway.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public void GetsAllFromTheGateway()
        {
            var stubbedEntities = _fixture.CreateMany<ConfirmTransferEntity>().ToList();
            _mockGateway.Setup(x => x.GetAll()).Returns(stubbedEntities);

            var expectedResponse = new ResponseObjectList { ResponseObjects = stubbedEntities.ToResponse() };

            _classUnderTest.Execute().Should().BeEquivalentTo(expectedResponse);
        }*/

        //TODO: Add extra tests here for extra functionality added to the use case
    }
}
