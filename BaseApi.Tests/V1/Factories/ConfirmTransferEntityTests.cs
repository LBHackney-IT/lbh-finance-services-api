using System.Linq;
using AutoFixture;
using BaseApi.V1.Boundary.Response;
using BaseApi.V1.Domain.SuspenseTransaction;
using BaseApi.V1.Factories;
using BaseApi.V1.Infrastructure;
using FluentAssertions;
using Xunit;


namespace BaseApi.Tests.V1.Factories
{

    public class ConfirmTransferEntityTests
    {
        private readonly Fixture _fixture = new Fixture();


        [Fact]
        public void ToDomainWithValidInputsReturnsConfirmTransferEntity()
        {
            AccountResponse accountResponse = _fixture.Create<AccountResponse>();
            TransactionResponse transactionResponse = _fixture.Create<TransactionResponse>();

            ConfirmTransferEntity confirmTransferEntity = EntityFactory.ToDomain(accountResponse, transactionResponse);
            confirmTransferEntity.Address.Should().BeEquivalentTo(transactionResponse.Address);
            confirmTransferEntity.ArrearsAfterPayment.Should().Be(accountResponse.AccountBalance - transactionResponse.TransactionAmount);
            confirmTransferEntity.CurrentArrears.Should().Be(accountResponse.AccountBalance);
            confirmTransferEntity.Payee.Should().Be(transactionResponse.Person.FullName);
            confirmTransferEntity.RentAccountNumber.Should().Be(accountResponse.PaymentReference);
            confirmTransferEntity.Resident.Should().Be(accountResponse.Tenure.PrimaryTenants.First().FullName);

        }

    }
}
