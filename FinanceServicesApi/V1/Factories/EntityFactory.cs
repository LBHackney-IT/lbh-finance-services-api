using System.Linq;
using FinanceServicesApi.V1.Boundary.Response;
using FinanceServicesApi.V1.Domain.SuspenseTransaction;

namespace FinanceServicesApi.V1.Factories
{
    public static class EntityFactory
    {
        public static ConfirmTransferEntity ToDomain(AccountResponse accountResponse, TransactionResponse transactionResponse)
        {
            return new ConfirmTransferEntity
            {
                Address = transactionResponse.Address,
                ArrearsAfterPayment = accountResponse.AccountBalance - transactionResponse.TransactionAmount,
                CurrentArrears = accountResponse.AccountBalance,
                Payee = transactionResponse.Person.FullName,
                RentAccountNumber = accountResponse.PaymentReference,
                Resident = accountResponse.Tenure.PrimaryTenants.First().FullName
            };
        }
    }
}
