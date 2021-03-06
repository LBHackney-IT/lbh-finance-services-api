using AutoFixture;
using FinanceServicesApi.V1.Domain.AccountModels;
using FinanceServicesApi.V1.Factories;
using FinanceServicesApi.V1.Infrastructure.Entities;
using FluentAssertions;
using Xunit;

namespace FinanceServicesApi.Tests.V1.Factories
{
    public class AccountFactoryTests
    {

        private readonly Fixture _fixture;

        public AccountFactoryTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void ToDomainWithCorrespondingDataTypeReturnsValidData()
        {
            AccountDbEntity dbEntity = _fixture.Create<AccountDbEntity>();
            Account domain = dbEntity.ToDomain();
            domain.Should().NotBeNull();
            dbEntity.Should().BeEquivalentTo(domain);
        }
    }
}
