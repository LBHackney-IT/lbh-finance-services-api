#nullable enable
using System;
using System.Collections.Generic;
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
        [MemberData(nameof(MockToResidentSummaryResponseInput.GetTestData), MemberType = typeof(MockToResidentSummaryResponseInput))]
        public void ToResidentSummaryResponseNeverReturnsNullOutput(
            Person person,
            TenureInformation tenure,
            Account account,
            Charge charge,
            List<ContactDetail> contactDetails,
            List<Transaction> transactions)
        {
            ResidentSummaryResponse response =
                ResponseFactory.ToResponse(person, tenure, account, charge, contactDetails, transactions);

            response.Should().NotBeNull();
        }

        [Fact]
        public void ToResidentSummaryResponseWithValidInputsReturnsValidOutPut()
        {
            Person person = _fixture.Create<Person>();
            TenureInformation tenureInformation = _fixture.Create<TenureInformation>();
            Account account = _fixture.Create<Account>();
            Charge charge = _fixture.Create<Charge>();
            List<ContactDetail> contacts = _fixture.Create<List<ContactDetail>>();
            List<Transaction> transactions = _fixture.Create<List<Transaction>>();

            ResidentSummaryResponse response =
                ResponseFactory.ToResponse(person, tenureInformation, account, charge, contacts, transactions);

            response.Should().NotBeNull();
        }

        [Theory]
        [ClassData(typeof(MockAccount))]
        public void ToResidentSummaryResponseWithAccountCircumstancesReturnsValidOutPut(
            Account account,
            decimal? balanceExpected,
            string tenureIdExpected)
        {
            Person person = _fixture.Create<Person>();
            TenureInformation tenureInformation = _fixture.Create<TenureInformation>();
            Charge charge = _fixture.Create<Charge>();
            List<ContactDetail> contacts = _fixture.Create<List<ContactDetail>>();
            List<Transaction> transactions = _fixture.Create<List<Transaction>>();

            ResidentSummaryResponse response =
                ResponseFactory.ToResponse(person, tenureInformation, account, charge, contacts, transactions);

            response.Should().NotBeNull();
            response.CurrentBalance.Should().Be(balanceExpected);
            response.TenureId.Should().Be(tenureIdExpected);
        }

        [Theory]
        [ClassData(typeof(MockTransaction))]
        public void ToResidentSummaryResponseWithTransactionCircumstancesReturnsValidOutPut(List<Transaction> transactions,
            decimal? housingBenefitExpected,
            decimal? lastPaymentAmountExpected,
            DateTime? lastPaymentDateExpected)
        {
            Person person = _fixture.Create<Person>();
            TenureInformation tenureInformation = _fixture.Create<TenureInformation>();
            Charge charge = _fixture.Create<Charge>();
            List<ContactDetail> contacts = _fixture.Create<List<ContactDetail>>();
            Account account = _fixture.Create<Account>();

            ResidentSummaryResponse response =
                ResponseFactory.ToResponse(person, tenureInformation, account, charge, contacts, transactions);

            response.Should().NotBeNull();
            response.HousingBenefit.Should().Be(housingBenefitExpected);
            response.LastPaymentAmount.Should().Be(lastPaymentAmountExpected);
            response.LastPaymentDate.Should().Be(lastPaymentDateExpected);
        }

        [Theory]
        [ClassData(typeof(MockPerson))]
        public void ToResidentSummaryResponseWithPersonCircumstancesReturnsValidOutPut(
            Person? person,
            DateTime? dateOfBirthExpected,
            string primaryTenantNameExpected)
        {
            Account account = _fixture.Create<Account>();
            TenureInformation tenureInformation = _fixture.Create<TenureInformation>();
            Charge charge = _fixture.Create<Charge>();
            List<ContactDetail> contacts = _fixture.Create<List<ContactDetail>>();
            List<Transaction> transactions = _fixture.CreateMany<Transaction>(5).ToList();

            ResidentSummaryResponse response =
                ResponseFactory.ToResponse(person, tenureInformation, account, charge, contacts, transactions);

            response.Should().NotBeNull();
            response.DateOfBirth.Should().Be(dateOfBirthExpected);
            response.PrimaryTenantName.Should().Be(primaryTenantNameExpected);
        }

        [Theory]
        [ClassData(typeof(MockTenure))]
        public void ToResidentSummaryResponseWithTenureCircumstancesReturnsValidOutPut(
            TenureInformation? tenure,
            string primaryTenantAddressExpected,
            string tenureTypeExpected,
            DateTime? tenureStartDateExpected,
            Guid tenureIdExpected)
        {
            Account account = _fixture.Create<Account>();
            Person person = _fixture.Create<Person>();
            Charge charge = _fixture.Create<Charge>();
            List<ContactDetail> contacts = _fixture.Create<List<ContactDetail>>();
            List<Transaction> transactions = _fixture.CreateMany<Transaction>(5).ToList();

            ResidentSummaryResponse response =
                ResponseFactory.ToResponse(person, tenure, account, charge, contacts, transactions);

            response.Should().NotBeNull();
            response.PrimaryTenantAddress.Should().Be(primaryTenantAddressExpected);
            response.TenureType.Should().Be(tenureTypeExpected);
            response.TenureStartDate.Should().Be(tenureStartDateExpected);
            response.Tenure.Id.Should().Be(tenureIdExpected);
        }

        [Theory]
        [ClassData(typeof(MockContact))]
        public void ToResidentSummaryResponseWithContactsCircumstancesReturnsValidOutPut(
            List<ContactDetail>? contacts,
            string primaryTenantEmailExpected,
            string primaryTenantPhoneNumberExpected)
        {
            Account account = _fixture.Create<Account>();
            Person person = _fixture.Create<Person>();
            Charge charge = _fixture.Create<Charge>();
            TenureInformation tenure = _fixture.Create<TenureInformation>();
            List<Transaction> transactions = _fixture.CreateMany<Transaction>(5).ToList();

            ResidentSummaryResponse response =
                ResponseFactory.ToResponse(person, tenure, account, charge, contacts, transactions);

            response.Should().NotBeNull();
            response.PrimaryTenantEmail.Should().Be(primaryTenantEmailExpected);
            response.PrimaryTenantPhoneNumber.Should().Be(primaryTenantPhoneNumberExpected);
        }

        /*[Theory]
        [ClassData(typeof(MockCharges))]
        public void ToResidentSummaryResponseWithChargesCircumstancesReturnsValidOutPut(
            Charge? charge,
            decimal? serviceChargeExpected,
            decimal? weeklyTotalChargesExpected)
        {
            Account account = _fixture.Create<Account>();
            Person person = _fixture.Create<Person>();
            List<ContactDetail> contacts = _fixture.Create<List<ContactDetail>>();
            TenureInformation tenure = _fixture.Create<TenureInformation>();
            List<Transaction> transactions = _fixture.CreateMany<Transaction>(5).ToList();

            ResidentSummaryResponse response =
                ResponseFactory.ToResponse(person, tenure, account, charge, contacts, transactions);

            response.Should().NotBeNull();
            response.ServiceCharge.Should().Be(serviceChargeExpected);
            response.WeeklyTotalCharges.Should().Be(weeklyTotalChargesExpected);
        }*/

    }
}
