using System;
using System.Threading.Tasks;
using AutoFixture;
using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.UseCase;
using FluentAssertions;
using Hackney.Shared.HousingSearch.Domain.Transactions;
using Moq;
using Xunit;
using TargetType = Hackney.Shared.HousingSearch.Domain.Transactions.TargetType;

namespace FinanceServicesApi.Tests.V1.UseCase
{
    public class GetTransactionByTargetIdUseCaseTests
    {
        private readonly GetTransactionByIdUseCase _sut;
        private readonly Mock<ITransactionGateway> _transactionGateway;
        private readonly Fixture _fixture;

        public GetTransactionByTargetIdUseCaseTests()
        {
            _transactionGateway = new Mock<ITransactionGateway>();
            _fixture = new Fixture();
            _sut = new GetTransactionByIdUseCase(_transactionGateway.Object);
        }

        [Fact]
        public void ExecuteAsyncWithValidIdReturnsAccount()
        {
            Guid id = Guid.NewGuid();

            Transaction transactionResponse = Transaction.Create(id
                , _fixture.Create<Guid>()
                , _fixture.Create<TargetType>()
                , _fixture.Create<short>()
                , _fixture.Create<short>()
                , _fixture.Create<short>()
                , _fixture.Create<string>()
                , _fixture.Create<TransactionType>()
                , _fixture.Create<DateTime>()
                , _fixture.Create<decimal>()
                , _fixture.Create<string>()
                , _fixture.Create<string>()
                , _fixture.Create<bool>()
                , SuspenseResolutionInfo.Create(_fixture.Create<DateTime>(), _fixture.Create<bool>(), _fixture.Create<bool>(), _fixture.Create<string>())
                , _fixture.Create<decimal>()
                , _fixture.Create<decimal>()
                , _fixture.Create<decimal>()
                , _fixture.Create<decimal>()
                , _fixture.Create<string>()
                , Sender.Create(_fixture.Create<Guid>(), _fixture.Create<string>())
                , _fixture.Create<string>()
                , _fixture.Create<DateTime>()
                , _fixture.Create<string>()
                , _fixture.Create<DateTime>()
                , _fixture.Create<string>());

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
