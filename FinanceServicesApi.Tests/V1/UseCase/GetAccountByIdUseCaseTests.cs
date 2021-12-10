using System;
using System.Threading.Tasks;
using AutoFixture;
using FinanceServicesApi.V1.Domain.AccountModels;
using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.UseCase;
using FluentAssertions;
using Moq;
using Xunit;

namespace FinanceServicesApi.Tests.V1.UseCase
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
        public void ExecuteAsyncWithValidIdReturnsAccount()
        {
            Account accountResponse = _fixture.Create<Account>();
            Guid id = Guid.NewGuid();

            _accountGateway.Setup(_ => _.GetById(It.IsAny<Guid>()))
                .ReturnsAsync(accountResponse);
            var response = _sut.ExecuteAsync(id);

            _accountGateway.Verify(_ => _.GetById(It.IsAny<Guid>()), Times.Once);
            response.Should().NotBeNull();
            response.Result.Should().BeEquivalentTo(accountResponse);
            response.Result.Id.Should().NotBeEmpty();
        }

        [Fact]
        public async Task ExecuteAsyncWithEmptyIdThrowsException()
        {
            Func<Task<Account>> func = async () => await _sut.ExecuteAsync(Guid.Empty).ConfigureAwait(false);
            await func.Should().ThrowAsync<Exception>().ConfigureAwait(false);
        }
    }
}
