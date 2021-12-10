using System;
using System.Threading.Tasks;
using AutoFixture;
using FinanceServicesApi.V1.Domain.TransactionModels;
using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.UseCase;
using FluentAssertions;
using Moq;
using Xunit;

namespace FinanceServicesApi.Tests.V1.UseCase
{
    public class GetTransactionByIdUseCaseTests
    {
        private readonly GetTransactionByIdUseCase _sut;
        private readonly Mock<ITransactionGateway> _transactionGateway;
        private readonly Fixture _fixture;

        public GetTransactionByIdUseCaseTests()
        {
            _transactionGateway = new Mock<ITransactionGateway>();
            _fixture = new Fixture();
            _sut = new GetTransactionByIdUseCase(_transactionGateway.Object);
        }

        [Fact]
        public void ExecuteAsyncWithValidIdReturnsAccount()
        {
            Transaction transactionResponse = _fixture.Create<Transaction>();
            Guid id = Guid.NewGuid();

            transactionResponse.Id = id;

            _transactionGateway.Setup(_ => _.GetById(It.IsAny<Guid>()))
                .ReturnsAsync(transactionResponse);
            var response = _sut.ExecuteAsync(id);

            _transactionGateway.Verify(_ => _.GetById(It.IsAny<Guid>()), Times.Once);
            response.Should().NotBeNull();
            response.Result.Should().BeEquivalentTo(transactionResponse);
            response.Result.Id.Should().Be(id);
        }

        [Fact]
        public async Task ExecuteAsyncWithEmptyIdThrowsException()
        {
            Func<Task<Transaction>> func = async () => await _sut.ExecuteAsync(Guid.Empty).ConfigureAwait(false);
            await func.Should().ThrowAsync<Exception>().ConfigureAwait(false);
        }
    }
}
