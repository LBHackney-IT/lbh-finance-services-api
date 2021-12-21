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

        [Fact]
        public void ToResponseWithValidAccountAndTransactionReturnsValidConfirmTransferResponse()
        {
            Account account = _fixture.Create<Account>();
            Transaction transaction = _fixture.Create<Transaction>();
            ConfirmTransferResponse confirmTransferResponse = ResponseFactory.ToResponse(account, transaction);
            confirmTransferResponse.Should().NotBeNull();
            confirmTransferResponse.Address.Should().Be(transaction.Address);
            if (account.AccountBalance - transaction.TransactionAmount < 0)
            {
                confirmTransferResponse.ArrearsAfterPayment.Should().Be(0);
            }
            else
            {
                confirmTransferResponse.ArrearsAfterPayment.Should().Be(account.AccountBalance - transaction.TransactionAmount);
            }
            confirmTransferResponse.CurrentArrears.Should().Be(account.AccountBalance);
            confirmTransferResponse.Payee.Should().Be(transaction.Person.FullName);
            confirmTransferResponse.RentAccountNumber.Should().Be(account.PaymentReference);
            confirmTransferResponse.Resident.Should().Be(account.Tenure.PrimaryTenants.First().FullName);
        }

        [Fact]
        public void ToResponseWithNullAccountAndValidTransactionReturnsValidConfirmTransferResponse()
        {
            Account account = null;
            Transaction transaction = _fixture.Create<Transaction>();
            ConfirmTransferResponse confirmTransferResponse = ResponseFactory.ToResponse(account, transaction);
            confirmTransferResponse.Should().NotBeNull();
            confirmTransferResponse.Address.Should().Be(transaction.Address);
            confirmTransferResponse.ArrearsAfterPayment.Should().Be(0);
            confirmTransferResponse.CurrentArrears.Should().Be(0);
            confirmTransferResponse.Payee.Should().Be(transaction.Person.FullName);
            confirmTransferResponse.RentAccountNumber.Should().BeNull();
            confirmTransferResponse.Resident.Should().BeNull();
        }

        [Fact]
        public void ToResponseWithValidAccountAndNullTransactionReturnsValidConfirmTransferResponse()
        {
            Account account = _fixture.Create<Account>();
            Transaction transaction = null;
            ConfirmTransferResponse confirmTransferResponse = ResponseFactory.ToResponse(account, transaction);
            confirmTransferResponse.Should().NotBeNull();
            confirmTransferResponse.Address.Should().BeNull();
            if (account.AccountBalance > 0)
            {
                confirmTransferResponse.ArrearsAfterPayment.Should().Be(account.AccountBalance);
                confirmTransferResponse.CurrentArrears.Should().Be(account.AccountBalance);
            }
            else
            {
                confirmTransferResponse.ArrearsAfterPayment.Should().Be(0);
                confirmTransferResponse.CurrentArrears.Should().Be(0);
            }
            confirmTransferResponse.Payee.Should().BeNull();
            confirmTransferResponse.RentAccountNumber.Should().Be(account.PaymentReference);
            confirmTransferResponse.Resident.Should().NotBeNull();
        }

        /*[Fact]
        public void ToResponseWithValidAccountWithNullTenureAndValidTransactionReturnsValidConfirmTransferResponse()
        {
            Account account = _fixture.Build<Account>()
                .Without(p => p.Tenure)
                .Create();

            Transaction transaction = _fixture.Create<Transaction>();
            ConfirmTransferResponse confirmTransferResponse = ResponseFactory.ToResponse(account, transaction);
            confirmTransferResponse.Address.Should().Be(transaction.Address);
            confirmTransferResponse.ArrearsAfterPayment.Should()
                .Be(account.AccountBalance - transaction.TransactionAmount);
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
            confirmTransferResponse.ArrearsAfterPayment.Should()
                .Be(account.AccountBalance - transaction.TransactionAmount);
            confirmTransferResponse.CurrentArrears.Should().Be(account.AccountBalance);
            confirmTransferResponse.Payee.Should().Be(transaction.Person.FullName);
            confirmTransferResponse.RentAccountNumber.Should().Be(account.PaymentReference);
            confirmTransferResponse.Resident.Should().BeNull();
        }*/
    }
}
