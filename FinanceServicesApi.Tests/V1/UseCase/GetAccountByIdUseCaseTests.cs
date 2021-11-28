using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.UseCase;
using FluentAssertions;
using Hackney.Shared.HousingSearch.Domain.Accounts;
using Hackney.Shared.HousingSearch.Domain.Accounts.Enum;
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
            Guid id = Guid.NewGuid();

            Account accountResponse = Account.Create(id
                , _fixture.Create<Guid>()
                , _fixture.Create<string>()
                , _fixture.Create<TargetType>()
                , _fixture.Create<Guid>()
                , _fixture.Create<AccountType>()
                , _fixture.Create<RentGroupType>()
                , _fixture.Create<string>()
                , _fixture.Create<decimal>()
                , _fixture.Create<decimal>()
                , _fixture.Create<string>()
                , _fixture.Create<string>()
                , _fixture.Create<DateTime>()
                , _fixture.Create<DateTime>()
                , _fixture.Create<DateTime>()
                , _fixture.Create<DateTime>()
                , _fixture.Create<AccountStatus>()
                , new List<ConsolidatedCharge>(2)
                {
                    {ConsolidatedCharge.Create(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<decimal>())},
                    {ConsolidatedCharge.Create(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<decimal>())}
                }
                , Tenure.Create(_fixture.Create<string>(), _fixture.Create<TenureType>(), _fixture.Create<string>(), new List<PrimaryTenant>(2)
                {
                    {PrimaryTenant.Create(_fixture.Create<Guid>(),_fixture.Create<string>())},
                    {PrimaryTenant.Create(_fixture.Create<Guid>(),_fixture.Create<string>())}
                }));

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
            Func<Task<Account>> func = async () => await _sut.ExecuteAsync(Guid.Empty).ConfigureAwait(false);
            await func.Should().ThrowAsync<Exception>().ConfigureAwait(false);
        }
    }
}
