#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoFixture;
using FinanceServicesApi.Tests.V1.Helper;
using FinanceServicesApi.V1.Boundary.Responses;
using FinanceServicesApi.V1.Boundary.Responses.ResidentSummary;
using FinanceServicesApi.V1.Domain.AccountModels;
using FinanceServicesApi.V1.Domain.Charges;
using FinanceServicesApi.V1.Domain.ContactDetails;
using FinanceServicesApi.V1.Domain.TransactionModels;
using FinanceServicesApi.V1.Factories;
using FluentAssertions;
using Hackney.Shared.Person;
using Hackney.Shared.Tenure.Domain;
using Xunit;

namespace FinanceServicesApi.Tests.V1.Factories
{
    public class ResponseFactoryTests
    {
        private readonly Fixture _fixture = new Fixture();

        #region ConfirmTransferResponse
        [Theory]
        [InlineData(100, 150, 0)]
        [InlineData(150, 100, 50)]
        [InlineData(150, 150, 0)]
        [InlineData(0, 0, 0)]
        public void ToConfirmTransferResponseReturnsValidArrears(decimal accountBalance, decimal transactionAmount, decimal arrearsAmount)
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
        public void ToConfirmTransferResponseWithNullPersonSubsetOfTransactionReturnsNullPayee()
        {
            var transaction = _fixture.Create<Transaction>();
            transaction.Person = null;
            var account = _fixture.Create<Account>();

            ConfirmTransferResponse confirmTransferResponse = ResponseFactory.ToResponse(account, transaction);
            confirmTransferResponse.Payee.Should().BeNull();
        }

        [Fact]
        public void ToConfirmTransferResponseWithNullTenureSubsetOfAccountReturnsNullResident()
        {
            var transaction = _fixture.Create<Transaction>();
            var account = _fixture.Create<Account>();
            account.Tenure = null;

            ConfirmTransferResponse confirmTransferResponse = ResponseFactory.ToResponse(account, transaction);
            confirmTransferResponse.Resident.Should().BeNull();
        }

        [Fact]
        public void ToConfirmTransferResponseWithValidAccountAndTransactionReturnsValidConfirmTransferResponse()
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
        public void ToConfirmTransferResponseWithNullAccountAndValidTransactionRaiseArgumentNullException()
        {
            Transaction transaction = _fixture.Create<Transaction>();
            Func<ConfirmTransferResponse> func = () => ResponseFactory.ToResponse(null, transaction);
            func.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ToConfirmTransferResponseWithValidAccountAndNullTransactionRaiseArgumentNullException()
        {
            Account account = _fixture.Create<Account>();
            Func<ConfirmTransferResponse> func = () => ResponseFactory.ToResponse(account, null);
            func.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ToConfirmTransferResponseWithValidAccountWithNullTenureAndValidTransactionReturnsValidConfirmTransferResponse()
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
        public void ToConfirmTransferResponseWithValidAccountWithNullPrimaryTenantsAndValidTransactionReturnsValidConfirmTransferResponse()
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
        #endregion


        [Theory]
        [MemberData(nameof(MockToResidentSummaryResponseInput.GetTestData),MemberType = typeof(MockToResidentSummaryResponseInput))]
        public void ToResidentSummaryResponseNeverReturnsNullOutput(Person person,TenureInformation tenure,Account account,List<Charge> charges,List<ContactDetail> contactDetails,List<Transaction> transactions)
        {
            ResidentSummaryResponse response =
                ResponseFactory.ToResponse(person, tenure, account, charges, contactDetails, transactions);

            response.Should().NotBeNull();
        }

        [Fact]
        public void ToResidentSummaryResponseWithValidInputsReturnsValidOutPut()
        {
            Person person = _fixture.Create<Person>();
            TenureInformation tenureInformation = _fixture.Create<TenureInformation>();
            Account account = _fixture.Create<Account>();
            List<Charge> charges = _fixture.Create<List<Charge>>();
            List<ContactDetail> contacts = _fixture.Create<List<ContactDetail>>();
            List<Transaction> transactions = _fixture.Create<List<Transaction>>();

            ResidentSummaryResponse response =
                ResponseFactory.ToResponse(person,tenureInformation, account, charges, contacts, transactions);

            response.Should().NotBeNull();
        }

        [Theory]
        /*[MemberData(nameof(MockAccount.GetTestData),MemberType = typeof(MockAccount))]*/
        [ClassData(typeof(MockAccount))]
        public void ToResidentSummaryResponseWithAccountReturnsValidOutPut(Account? account,decimal? balanceExpected,string tenureIdExpected)
        {
            Person person = _fixture.Create<Person>();
            TenureInformation tenureInformation = _fixture.Create<TenureInformation>();
            List<Charge> charges = _fixture.Create<List<Charge>>();
            List<ContactDetail> contacts = _fixture.Create<List<ContactDetail>>();
            List<Transaction> transactions = _fixture.Create<List<Transaction>>();

            ResidentSummaryResponse response =
                ResponseFactory.ToResponse(person, tenureInformation, account, charges, contacts, transactions);

            response.Should().NotBeNull();
            response.CurrentBalance.Should().Be(balanceExpected);
            response.TenureId.Should().Be(tenureIdExpected);
        }

    }
}
