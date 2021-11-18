using System;
using System.Collections.Generic;
using System.Linq;
using FinanceServicesApi.V1.Boundary.Response;
using FinanceServicesApi.V1.Domain;
using FinanceServicesApi.V1.Domain.ContactDetails;
using FinanceServicesApi.V1.Domain.FinancialSummary;
using Hackney.Shared.Person;
using Hackney.Shared.Tenure.Domain;

namespace FinanceServicesApi.V1.Factories
{
    public static class ResponseFactory
    {
        public static ConfirmTransferResponse ToResponse(Account account, Transaction transaction)
        {
            return new ConfirmTransferResponse
            {
                Address = transaction.Address,
                ArrearsAfterPayment = account.AccountBalance - transaction.TransactionAmount,
                CurrentArrears = account.AccountBalance,
                Payee = transaction.Person.FullName,
                RentAccountNumber = account.PaymentReference,
                Resident = account.Tenure.PrimaryTenants.First().FullName
            };
        }

        public static ResidentSummaryResponse ToResponse(Person person,List<TenureInformation> tenures,List<Charges> charges,List<ContactDetails> contacts,List<WeeklySummary> summaries,List<Transaction> transactions)
        {
            if (summaries == null) throw new ArgumentNullException(nameof(summaries));
            if (person == null) throw new ArgumentNullException(nameof(person));
            if (tenures == null) throw new ArgumentNullException(nameof(tenures));
            if (charges == null) throw new ArgumentNullException(nameof(charges));
            if (contacts == null) throw new ArgumentNullException(nameof(contacts));
            return  new ResidentSummaryResponse
            {
                CurrentBalance = summaries.Sum(p=>p.BalanceAmount),
                HousingBenefit = summaries.Sum(p=>p.HousingBenefitAmount),
                ServiceCharge = summaries.Sum(p=>p.ChargedAmount),
                DateOfBirth = person.DateOfBirth,
                PersonId = "Not Detected",
                LastPaymentAmount = transactions.Select(p=>p.PaidAmount).Last(),
                LastPaymentDate = transactions.Where(p=>p.PaidAmount>0).Select(p => p.TransactionDate).Last()
            };
        }
    }
}
