using AutoFixture;
using BaseApi.V1.Boundary.Response;
using BaseApi.V1.Domain;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BaseApi.Tests.V1.Boundary.Response
{
    public class TransactionResponseTests
    {
        private readonly Fixture _fixture = new Fixture();

        [Fact]
        public void ResponseHasPropertiesSet()
        {
            var transactionResponseType = typeof(TransactionResponse);
            transactionResponseType.GetProperties().Length.Should().Be(20);

            TransactionResponse transactionResponse = _fixture.Create<TransactionResponse>();

            Assert.IsType<string>(transactionResponse.Address);
            Assert.IsType<decimal>(transactionResponse.BalanceAmount);
            Assert.IsType<string>(transactionResponse.BankAccountNumber);
            Assert.IsType<decimal>(transactionResponse.ChargedAmount);
            Assert.IsType<int>(transactionResponse.FinancialMonth);
            Assert.IsType<int>(transactionResponse.FinancialYear);
            Assert.IsType<string>(transactionResponse.Fund);
            Assert.IsType<decimal>(transactionResponse.HousingBenefitAmount);
            Assert.IsType<Guid>(transactionResponse.Id);
            Assert.IsType<bool>(transactionResponse.IsSuspense);
            Assert.IsType<decimal>(transactionResponse.PaidAmount);
            Assert.IsType<string>(transactionResponse.PaymentReference);
            Assert.IsType<short>(transactionResponse.PeriodNo);
            Assert.IsType<Guid>(transactionResponse.TargetId);
            Assert.IsType<decimal>(transactionResponse.TransactionAmount);
            Assert.IsType<DateTime>(transactionResponse.TransactionDate);
            Assert.IsType<string>(transactionResponse.TransactionSource);

            Assert.IsType<Person>(transactionResponse.Person);
            var personType = typeof(Person);
            personType.GetProperties().Length.Should().Be(2);
            Assert.IsType<Guid>(transactionResponse.Person.Id);
            Assert.IsType<string>(transactionResponse.Person.FullName);
            

            Assert.IsType<SuspenseResolutionInfo>(transactionResponse.SuspenseResolutionInfo);
            var suspenseResolutionInfoType = typeof(SuspenseResolutionInfo);
            suspenseResolutionInfoType.GetProperties().Length.Should().Be(7);
            Assert.IsType<DateTime>(transactionResponse.SuspenseResolutionInfo.ApprovedDate);
            Assert.IsType<DateTime>(transactionResponse.SuspenseResolutionInfo.ConfirmedDate);
            Assert.IsType<DateTime>(transactionResponse.SuspenseResolutionInfo.ResolutionDate);
            Assert.IsType<bool>(transactionResponse.SuspenseResolutionInfo.IsApproved);
            Assert.IsType<bool>(transactionResponse.SuspenseResolutionInfo.IsConfirmed);
            Assert.IsType<bool>(transactionResponse.SuspenseResolutionInfo.IsResolve);
            Assert.IsType<string>(transactionResponse.SuspenseResolutionInfo.Note);

            Assert.IsType<TransactionType>(transactionResponse.TransactionType);
            var transactionType = typeof(TransactionType);
            Enum.GetValues(transactionType).Length.Should().Be(2);

        }
    }
}
