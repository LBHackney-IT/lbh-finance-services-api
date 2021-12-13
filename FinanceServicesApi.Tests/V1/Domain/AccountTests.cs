using System;
using System.Collections.Generic;
using AutoFixture;
using FinanceServicesApi.V1.Domain.AccountModels;
using FinanceServicesApi.V1.Infrastructure.Enums;
using FluentAssertions;
using Xunit;

namespace FinanceServicesApi.Tests.V1.Domain
{
    public class AccountTests
    {
        private readonly Fixture _fixture;

        public AccountTests()
        {
            _fixture = new Fixture();
        }


        [Fact]
        public void AccountModelHasPropertiesSet()
        {
            #region AccountModel
            var model = typeof(Account);
            model.GetProperties().Length.Should().Be(20);

            Account account = _fixture.Create<Account>();

            Assert.IsType<Guid>(account.Id);
            Assert.IsType<AccountStatus>(account.AccountStatus);
            Assert.IsType<AccountType>(account.AccountType);
            Assert.IsType<string>(account.AgreementType);
            Assert.IsType<string>(account.CreatedBy);
            Assert.IsType<DateTime>(account.CreatedAt);
            Assert.IsType<DateTime>(account.EndDate);
            Assert.IsType<string>(account.LastUpdatedBy);
            Assert.IsType<DateTime>(account.LastUpdatedAt);
            Assert.IsType<Guid>(account.ParentAccountId);
            Assert.IsType<string>(account.PaymentReference);
            Assert.IsType<RentGroupType>(account.RentGroupType);
            Assert.IsType<DateTime>(account.StartDate);
            Assert.IsType<Guid>(account.TargetId);
            Assert.IsType<TargetType>(account.TargetType);
            Assert.IsAssignableFrom<IEnumerable<ConsolidatedCharge>>(account.ConsolidatedCharges);
            Assert.IsType<AccountTenureSubSet>(account.Tenure);
            Assert.IsType<decimal>(account.ConsolidatedBalance);
            #endregion

            #region ConsolidatedCharge
            var consolidatedChargeEntity = typeof(ConsolidatedCharge);
            consolidatedChargeEntity.GetProperties().Length.Should().Be(3);

            ConsolidatedCharge consolidatedCharge = _fixture.Create<ConsolidatedCharge>();

            Assert.IsType<decimal>(consolidatedCharge.Amount);
            Assert.IsType<string>(consolidatedCharge.Frequency);
            Assert.IsType<string>(consolidatedCharge.Type);
            #endregion

            #region Tenure
            var entityTenure = typeof(AccountTenureSubSet);
            entityTenure.GetProperties().Length.Should().Be(4);

            AccountTenureSubSet tenure = _fixture.Create<AccountTenureSubSet>();
            Assert.IsType<string>(tenure.FullAddress);
            Assert.IsType<string>(tenure.TenureId);
            Assert.IsType<TenureType>(tenure.TenureType);
            Assert.IsAssignableFrom<IEnumerable<PrimaryTenants>>(tenure.PrimaryTenants);
            #endregion

            #region PrimaryTenant
            var entityPrimaryTenant = typeof(PrimaryTenants);
            entityPrimaryTenant.GetProperties().Length.Should().Be(2);

            PrimaryTenants primaryTenant = _fixture.Create<PrimaryTenants>();
            Assert.IsType<string>(primaryTenant.FullName);
            Assert.IsType<Guid>(primaryTenant.Id);
            #endregion
        }
    }
}
