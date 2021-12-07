using AutoFixture;
using FluentAssertions;
using System;
using Hackney.Shared.HousingSearch.Domain.Transactions;
using Xunit;

namespace FinanceServicesApi.Tests.V1.Domain
{
    public class TransactionTests
    {
        private readonly Fixture _fixture = new Fixture();

        [Fact]
        public void ResponseHasPropertiesSet()
        {
            var transactionResponseType = typeof(Transaction);
            transactionResponseType.GetProperties().Length.Should().Be(20);

            Transaction transaction = _fixture.Create<Transaction>();

            Assert.IsType<string>(transaction.Address);
            Assert.IsType<decimal>(transaction.BalanceAmount);
            Assert.IsType<string>(transaction.BankAccountNumber);
            Assert.IsType<decimal>(transaction.ChargedAmount);
            Assert.IsType<int>(transaction.FinancialMonth);
            Assert.IsType<int>(transaction.FinancialYear);
            Assert.IsType<string>(transaction.Fund);
            Assert.IsType<decimal>(transaction.HousingBenefitAmount);
            Assert.IsType<Guid>(transaction.Id);
            Assert.IsType<bool>(transaction.IsSuspense);
            Assert.IsType<decimal>(transaction.PaidAmount);
            Assert.IsType<string>(transaction.PaymentReference);
            Assert.IsType<short>(transaction.PeriodNo);
            Assert.IsType<Guid>(transaction.TargetId);
            Assert.IsType<decimal>(transaction.TransactionAmount);
            Assert.IsType<DateTime>(transaction.TransactionDate);
            Assert.IsType<string>(transaction.TransactionSource);

            Assert.IsType<Sender>(transaction.Sender);
            var personType = typeof(Sender);
            personType.GetProperties().Length.Should().Be(2);
            Assert.IsType<Guid>(transaction.Sender.Id);
            Assert.IsType<string>(transaction.Sender.FullName);


            Assert.IsType<SuspenseResolutionInfo>(transaction.SuspenseResolutionInfo);
            var suspenseResolutionInfoType = typeof(SuspenseResolutionInfo);
            suspenseResolutionInfoType.GetProperties().Length.Should().Be(700);
            /*Assert.IsType<DateTime>(transaction.SuspenseResolutionInfo.ApprovedDate);
            Assert.IsType<DateTime>(transaction.SuspenseResolutionInfo.ConfirmedDate);*/
            Assert.IsType<DateTime>(transaction.SuspenseResolutionInfo.ResolutionDate);
            Assert.IsType<bool>(transaction.SuspenseResolutionInfo.IsApproved);
            Assert.IsType<bool>(transaction.SuspenseResolutionInfo.IsConfirmed);
            Assert.IsType<bool>(transaction.SuspenseResolutionInfo.IsResolve);
            Assert.IsType<string>(transaction.SuspenseResolutionInfo.Note);

            Assert.IsType<TransactionType>(transaction.TransactionType);
            var transactionType = typeof(TransactionType);
            Enum.GetValues(transactionType).Length.Should().Be(2);

        }
    }
}
