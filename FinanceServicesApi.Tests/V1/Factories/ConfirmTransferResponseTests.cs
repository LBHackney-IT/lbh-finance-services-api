using System.Linq;
using AutoFixture;
using FinanceServicesApi.V1.Boundary.Responses;
using FinanceServicesApi.V1.Domain.ContactDetails;
using FinanceServicesApi.V1.Factories;
using FluentAssertions;
using Hackney.Shared.HousingSearch.Domain.Accounts;
using Hackney.Shared.HousingSearch.Domain.Transactions;
using Xunit;


namespace FinanceServicesApi.Tests.V1.Factories
{

    public class ConfirmTransferResponseTests
    {
        private readonly Fixture _fixture = new Fixture();

        [Fact]
        public void ToDomainWithValidInputsReturnsConfirmTransferEntity()
        {
            Account accountResponse = _fixture.Create<Account>();
            Transaction transactionResponse = _fixture.Create<Transaction>();
            
            ConfirmTransferResponse confirmTransferResponse = ResponseFactory.ToResponse(accountResponse, transactionResponse);
            confirmTransferResponse.Address.Should().BeEquivalentTo(transactionResponse.Address);
            confirmTransferResponse.ArrearsAfterPayment.Should().Be(accountResponse.AccountBalance - transactionResponse.TransactionAmount);
            confirmTransferResponse.CurrentArrears.Should().Be(accountResponse.AccountBalance);
            confirmTransferResponse.Payee.Should().Be(transactionResponse.Sender.FullName);
            confirmTransferResponse.RentAccountNumber.Should().Be(accountResponse.PaymentReference);
            confirmTransferResponse.Resident.Should().Be(accountResponse.Tenure.PrimaryTenants.First().FullName);

        }

    }
}
