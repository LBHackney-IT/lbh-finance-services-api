using System;
using System.Collections.Generic;
using System.Linq;
using FinanceServicesApi.V1.Boundary.Responses;
using FinanceServicesApi.V1.Domain.ContactDetails;
using FinanceServicesApi.V1.Domain.FinancialSummary;
using Hackney.Shared.HousingSearch.Domain.Accounts;
using Hackney.Shared.HousingSearch.Domain.Transactions;
using Hackney.Shared.Tenure.Domain;
using Person = Hackney.Shared.HousingSearch.Domain.Person.Person;
using TargetType = FinanceServicesApi.V1.Domain.ContactDetails.TargetType;

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
                Payee = transaction.Sender.FullName,
                RentAccountNumber = account.PaymentReference,
                Resident = account.Tenure.PrimaryTenants.First().FullName
            };
        }

        public static ResidentSummaryResponse ToResponse(Person person
            , TenureInformation tenure
            , Account accounts
            , Domain.Charges.Charge charges
            , List<ContactDetails> contacts
            , List<WeeklySummary> summaries
            , List<Transaction> transactions)
        {
            if (summaries == null) throw new ArgumentNullException(nameof(summaries));
            if (transactions == null) throw new ArgumentNullException(nameof(transactions));
            if (person == null) throw new ArgumentNullException(nameof(person));
            if (tenure == null) throw new ArgumentNullException(nameof(tenure));
            if (accounts == null) throw new ArgumentNullException(nameof(accounts));
            if (charges == null) throw new ArgumentNullException(nameof(charges));
            if (contacts == null) throw new ArgumentNullException(nameof(contacts));

            /*var masterAccount = accounts.First(a => a.AccountType == AccountType.Master);*/

            return new ResidentSummaryResponse
            {
                CurrentBalance = accounts?.ConsolidatedBalance ?? 0,
                HousingBenefit = summaries.Sum(s=>s.HousingBenefitAmount),
                ServiceCharge = charges.DetailedCharges.Where(c=>c.Type.ToLower()=="service").Sum(c=>c.Amount),
                DateOfBirth = DateTime.Parse(person.DateOfBirth),
                PersonId = "Not Detected",
                LastPaymentAmount = transactions.Last().PaidAmount,
                LastPaymentDate = transactions.Last(p => p.PaidAmount > 0).TransactionDate,
                PrimaryTenantAddress = tenure.TenuredAsset.FullAddress,
                TenancyType = tenure.TenureType.Code,
                PrimaryTenantEmail = contacts.Where(c=>c.TargetType==TargetType.Person && c.ContactInformation.ContactType==ContactType.Email)
                    .Select(s=>s.ContactInformation.Value).First(),
                PrimaryTenantName = accounts?.Tenure.PrimaryTenants.First().FullName,
                PrimaryTenantPhoneNumber = contacts.Where(c => c.TargetType == TargetType.Person && c.ContactInformation.ContactType == ContactType.Phone)
                    .Select(s => s.ContactInformation.Value).First(),
                TenureId = "Not Detected",
                TenureStartDate = tenure.StartOfTenureDate,
                WeeklyTotalCharges = charges.DetailedCharges.Where(c=>c.Type.ToLower()== "weekly").Sum(c=>c.Amount)
            };
        }
    }
}
