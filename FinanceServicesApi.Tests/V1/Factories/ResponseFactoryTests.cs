using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoFixture;
using FinanceServicesApi.Tests.V1.Helper;
using FinanceServicesApi.V1.Boundary.Responses;
using FinanceServicesApi.V1.Domain.AccountModels;
using FinanceServicesApi.V1.Domain.TransactionModels;
using FinanceServicesApi.V1.Factories;
using FluentAssertions;
using Xunit;

namespace FinanceServicesApi.Tests.V1.Factories
{
    public class ResponseFactoryTests
    {
        private readonly Fixture _fixture = new Fixture();

        [Theory]
        [InlineData(100,150,0)]
        [InlineData(150,100,50)]
        [InlineData(150,150,0)]
        [InlineData(0,0,0)]
        public void ToResponseReturnsValidArrears(decimal accountBalance,decimal transactionAmount,decimal arrearsAmount)
        {
            Account account = _fixture.Build<Account>()
                .With(p => p.AccountBalance, accountBalance)
                .Create();
            Transaction transaction = _fixture.Build<Transaction>()
                .With(p => p.TransactionAmount, transactionAmount)
                .Create();

            ConfirmTransferResponse confirmTransferResponse = ResponseFactory.ToResponse(account, transaction);
            confirmTransferResponse.CurrentArrears.Should().Be(account.AccountBalance);
            confirmTransferResponse.ArrearsAfterPayment.Should().Be(arrearsAmount);
        }

        [Fact]
        public void ToResponseWithNullPersonSubsetOfTransactionReturnsNullPayee()
        {
            var transaction = _fixture.Create<Transaction>();
            transaction.Person = null;
            var account = _fixture.Create<Account>();

            ConfirmTransferResponse confirmTransferResponse = ResponseFactory.ToResponse(account, transaction);
            confirmTransferResponse.Payee.Should().BeNull();
        }

        [Fact]
        public void ToResponseWithNullTenureSubsetOfAccountReturnsNullResident()
        {
            var transaction = _fixture.Create<Transaction>();
            var account = _fixture.Create<Account>();
            account.Tenure = null;

            ConfirmTransferResponse confirmTransferResponse = ResponseFactory.ToResponse(account, transaction);
            confirmTransferResponse.Resident.Should().BeNull();
        }

        [Fact]
        public void ToResponseWithValidAccountAndTransactionReturnsValidConfirmTransferResponse()
        {
            Account account = _fixture.Create<Account>();
            Transaction transaction = _fixture.Create<Transaction>();

            ConfirmTransferResponse confirmTransferResponse = ResponseFactory.ToResponse(account, transaction);
            confirmTransferResponse.Should().NotBeNull();
            confirmTransferResponse.Address.Should().Be(transaction.Address);
            confirmTransferResponse.Payee.Should().Be(transaction.Person.FullName);
            confirmTransferResponse.RentAccountNumber.Should().Be(account.PaymentReference);
            confirmTransferResponse.Resident.Should().Be(account.Tenure.PrimaryTenants.First().FullName);
        }

        [Fact]
        public void ToResponseWithNullAccountAndValidTransactionRaiseArgumentNullException()
        {
            Transaction transaction = _fixture.Create<Transaction>();
            Func<ConfirmTransferResponse> func =()=> ResponseFactory.ToResponse(null, transaction);
            func.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ToResponseWithValidAccountAndNullTransactionRaiseArgumentNullException()
        {
            Account account = _fixture.Create<Account>();
            Func<ConfirmTransferResponse> func =()=> ResponseFactory.ToResponse(account, null);
            func.Should().Throw<ArgumentNullException>();
        }
        
        [Fact]
        public void ToResponseWithValidAccountWithNullTenureAndValidTransactionReturnsValidConfirmTransferResponse()
        {
            Account account = _fixture.Build<Account>()
                .Without(p => p.Tenure)
                .Create();

            Transaction transaction = _fixture.Create<Transaction>();
            ConfirmTransferResponse confirmTransferResponse = ResponseFactory.ToResponse(account, transaction);
            confirmTransferResponse.Address.Should().Be(transaction.Address);
            confirmTransferResponse.CurrentArrears.Should().Be(account.AccountBalance);
            confirmTransferResponse.Payee.Should().Be(transaction.Person.FullName);
            confirmTransferResponse.RentAccountNumber.Should().Be(account.PaymentReference);
            confirmTransferResponse.Resident.Should().BeNull();
        }

        [Fact]
        public void ToResponseWithValidAccountWithNullPrimaryTenantsAndValidTransactionReturnsValidConfirmTransferResponse()
        {
            Account account = _fixture.Create<Account>();
            account.Tenure.PrimaryTenants = null;
            Transaction transaction = _fixture.Create<Transaction>();

            ConfirmTransferResponse confirmTransferResponse = ResponseFactory.ToResponse(account, transaction);
            confirmTransferResponse.Address.Should().Be(transaction.Address);
            confirmTransferResponse.Payee.Should().Be(transaction.Person.FullName);
            confirmTransferResponse.RentAccountNumber.Should().Be(account.PaymentReference);
            confirmTransferResponse.Resident.Should().BeNull();
        }
    }
}
