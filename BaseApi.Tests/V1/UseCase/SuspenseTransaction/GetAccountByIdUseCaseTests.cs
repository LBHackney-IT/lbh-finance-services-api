using System;
using System.Threading.Tasks;
using AutoFixture;
using BaseApi.V1.Boundary.Response;
using BaseApi.V1.Gateways.Interfaces.SuspenseTransaction;
using BaseApi.V1.UseCase.SuspenseTransaction;
using FluentAssertions;
using Moq;
using Xunit;

namespace BaseApi.Tests.V1.UseCase.SuspenseTransaction
{
    public class GetAccountByIdUseCaseTests
    {
        private readonly GetAccountByIdUseCase _sut;
        private readonly Mock<IAccountGateway> _accountGateway;
        private readonly Fixture _fixture;

        public GetAccountByIdUseCaseTests()
        {
            _accountGateway = new Mock<IAccountGateway>();
            _fixture = new Fixture();
            _sut = new GetAccountByIdUseCase(_accountGateway.Object);
        }

        [Fact]
        public void ExecuteAsyncWithValidIdReturnsAccountResponse()
        {
            AccountResponse accountResponse = _fixture.Create<AccountResponse>();
            Guid id = Guid.NewGuid();

            accountResponse.Id = id;

            _accountGateway.Setup(_ => _.GetById(It.IsAny<Guid>()))
                .ReturnsAsync(accountResponse);
            var response = _sut.ExecuteAsync(id);

            _accountGateway.Verify(_ => _.GetById(It.IsAny<Guid>()), Times.Once);
            response.Should().NotBeNull();
            response.Result.Should().BeEquivalentTo(accountResponse);
            response.Result.Id.Should().Be(id);
        }

        [Fact]
        public async Task ExecuteAsyncWithEmptyIdThrowsException()
        {
            Func<Task<AccountResponse>> func = async () => await _sut.ExecuteAsync(Guid.Empty).ConfigureAwait(false);
            await func.Should().ThrowAsync<Exception>().ConfigureAwait(false);
        }
    }
}
