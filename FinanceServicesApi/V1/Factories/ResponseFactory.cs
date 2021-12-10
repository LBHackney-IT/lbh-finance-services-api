using System;
using System.Collections.Generic;
using System.Linq;
using FinanceServicesApi.V1.Boundary.Responses;
using FinanceServicesApi.V1.Boundary.Responses.ResidentSummary;
using FinanceServicesApi.V1.Domain.AccountModels;
using FinanceServicesApi.V1.Domain.ContactDetails;
using FinanceServicesApi.V1.Domain.FinancialSummary;
using FinanceServicesApi.V1.Domain.TransactionModels;
using FinanceServicesApi.V1.Infrastructure.Enums;
using Hackney.Shared.Asset.Domain;
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

        public static ResidentSummaryResponse ToResponse(Person person, TenureInformation tenure, Account account, List<Domain.Charges.Charge> charges, List<ContactDetail> contacts, List<WeeklySummary> summaries, List<Transaction> transactions)
        {
            /*if (summaries == null) throw new ArgumentNullException(nameof(summaries));
            if (person == null) throw new ArgumentNullException(nameof(person));
            if (tenure == null) throw new ArgumentNullException(nameof(tenure));
            if (account == null) throw new ArgumentNullException(nameof(account));
            if (charges == null) throw new ArgumentNullException(nameof(charges));
            if (contacts == null) throw new ArgumentNullException(nameof(contacts));*/

            return new ResidentSummaryResponse
            {
                CurrentBalance = account?.ConsolidatedBalance,
                HousingBenefit = summaries.Count == 0 ? 0 : summaries.OrderBy(o => o.SubmitDate).Last().HousingBenefitAmount,
                ServiceCharge = charges.Count == 0 ? 0 : charges.Sum(p => p.DetailedCharges.Where(c => c.Type.ToLower() == "service").Sum(c => c.Amount)),
                DateOfBirth = person?.DateOfBirth,
                PersonId = "-",
                TenureId = "-",
                LastPaymentAmount = transactions.Count == 0 ? 0 : transactions.Last().PaidAmount,
                LastPaymentDate = transactions.Count == 0 ? (DateTime?) null : transactions.Last(p => p.PaidAmount > 0).TransactionDate,
                PrimaryTenantAddress = tenure?.TenuredAsset?.FullAddress,
                TenancyType = tenure?.TenureType.Code,
                PrimaryTenantEmail = contacts.Where(c => c.TargetType == TargetType.Person && c.ContactInformation.ContactType == ContactType.Email)
                    .Select(s => s.ContactInformation.Value).FirstOrDefault() ?? "",
                PrimaryTenantName = account?.Tenure.PrimaryTenants.First().FullName,
                PrimaryTenantPhoneNumber = contacts.Where(c => c.TargetType == TargetType.Person && c.ContactInformation.ContactType == ContactType.Phone)
                    .Select(s => s.ContactInformation.Value).FirstOrDefault() ?? "",
                TenureStartDate = tenure?.StartOfTenureDate,
                WeeklyTotalCharges = charges.Sum(p => p.DetailedCharges.Where(c => c.Type.ToLower() == "weekly").Sum(c => c.Amount))
            };
        }

        public static ResidentAssetsResponse ToResponse(Asset asset, TenureInformation tenure)
        {
            return new ResidentAssetsResponse()
            {
                AssetAddress = asset?.AssetAddress,
                AssetType = asset?.AssetType,
                RentAccountNumber = tenure?.PaymentReference,
                CurrentBalance = tenure?.Charges?.CurrentBalance,
                RentCharge = tenure?.Charges?.Rent,
                ServiceCharge = tenure?.Charges?.ServiceCharge,
                Staircasing = -1,
                TenancyType = tenure?.TenureType
            };
        }
    }
}
