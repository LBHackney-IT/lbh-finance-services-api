using System.Linq;
using AutoFixture;
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
        //private readonly Fixture _fixture = new Fixture();

        //[Fact]
        //public void ToResponseWithValidAccountAndTransactionReturnsValidConfirmTransferResponse()
        //{
        //    Account account = _fixture.Create<Account>();
        //    Transaction transaction = _fixture.Create<Transaction>();
        //    ConfirmTransferResponse confirmTransferResponse = ResponseFactory.ToResponse(account, transaction);
        //    confirmTransferResponse.Address.Should().Be(transaction.Address);
        //    confirmTransferResponse.ArrearsAfterPayment.Should()
        //        .Be(account.AccountBalance - transaction.TransactionAmount);
        //    confirmTransferResponse.CurrentArrears.Should().Be(account.AccountBalance);
        //    confirmTransferResponse.Payee.Should().Be(transaction.Person.FullName);
        //    confirmTransferResponse.RentAccountNumber.Should().Be(account.PaymentReference);
        //    confirmTransferResponse.Resident.Should().Be(account.Tenure.PrimaryTenants.First().FullName);
        //}


    }
}
