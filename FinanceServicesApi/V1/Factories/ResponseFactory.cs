using System;
using System.Collections.Generic;
using System.Linq;
using FinanceServicesApi.V1.Boundary.Responses;
using FinanceServicesApi.V1.Boundary.Responses.PropertySummary;
using FinanceServicesApi.V1.Boundary.Responses.ResidentSummary;
using FinanceServicesApi.V1.Domain.AccountModels;
using FinanceServicesApi.V1.Domain.Charges;
using FinanceServicesApi.V1.Domain.ContactDetails;
using FinanceServicesApi.V1.Domain.TenureModels;
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
            if (account == null) throw new ArgumentNullException(nameof(account));
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            var arrearsAfterPayment = account.AccountBalance + transaction.TransactionAmount;
            return new ConfirmTransferResponse
            {
                Address = transaction.Address,
                ArrearsAfterPayment = arrearsAfterPayment < 0 ? 0 : arrearsAfterPayment,
                CurrentArrears = account.AccountBalance,
                Payee = transaction.Person?.FullName,
                RentAccountNumber = account.PaymentReference,
                Resident = account.Tenure?.PrimaryTenants?.FirstOrDefault()?.FullName,
                TotalAmount = transaction.TransactionAmount
            };
        }

        public static ResidentSummaryResponse ToResponse(Person person,
            TenureInformation tenure,
            Account account,
            Domain.Charges.Charge charge,
            List<ContactDetail> contacts,
            List<Transaction> transactions,
            bool isLeaseHolder)
        {
            return new ResidentSummaryResponse
            {
                CurrentBalance = account?.ConsolidatedBalance,
                TenureId = account?.Tenure?.TenureId,

                HousingBenefit = transactions?.Sum(s => s.HousingBenefitAmount),
                LastPaymentAmount = transactions?.Count == 0 ? 0 : transactions?.LastOrDefault(p => p.PaidAmount != 0)?.PaidAmount ?? 0,
                LastPaymentDate = transactions?.Count == 0 ? (DateTime?) null : transactions?.LastOrDefault(p => p.PaidAmount != 0)?.TransactionDate,

                ServiceCharge = isLeaseHolder
                    ? charge?.DetailedCharges?.Where(c => c.Type.ToLower() == "service").Sum(c => c.Amount) / 12
                    : charge?.DetailedCharges?.Where(c => c.Type.ToLower() == "service").Sum(c => c.Amount),
                WeeklyTotalCharges = charge?.DetailedCharges?.Where(c =>
                        c.Frequency.ToLower() == "weekly").Sum(c => c.Amount),

                DateOfBirth = person?.DateOfBirth,
                PrimaryTenantName = (person?.FirstName == null && person?.Surname == null) ? null :
                    ($"{person?.FirstName ?? ""} {person?.Surname ?? ""}").Trim(),
                PersonId = "-",

                PrimaryTenantAddress = tenure?.TenuredAsset?.FullAddress,
                TenureType = tenure?.TenureType?.Description,
                TenureStartDate = tenure?.StartOfTenureDate,
                Tenure = new TenurePartialModel { Id = tenure?.Id ?? Guid.Empty },

                PrimaryTenantEmail = contacts?.FirstOrDefault(c =>
                        c.TargetType == TargetType.Person &&
                        c.ContactInformation?.ContactType == ContactType.Email)
                    ?.ContactInformation?.Value,
                PrimaryTenantPhoneNumber = contacts?.FirstOrDefault(c =>
                        c.TargetType == TargetType.Person &&
                        c.ContactInformation?.ContactType == ContactType.Phone)
                    ?.ContactInformation?.Value
            };
        }

        public static PropertySummaryResponse ToResponse(TenureInformation tenure,
            Person person,
            Account account,
            Charge charges,
            List<ContactDetail> contacts,
            List<Transaction> transactions,
            Asset asset,
            bool isLeaseholder)
        {
            var financialYear = DateTime.UtcNow.Year + ((DateTime.UtcNow.Month > 0 && DateTime.UtcNow.Month < 4) ? -1 : 0);
            var firstDayOfFinancialYear = new DateTime(financialYear, 4, 1);
            while (firstDayOfFinancialYear.DayOfWeek != DayOfWeek.Monday)
            {
                firstDayOfFinancialYear = firstDayOfFinancialYear.AddDays(1);
            }
            var lastDayOfFinancialYear = firstDayOfFinancialYear.AddYears(1).AddDays(-1);

            var yearToDate = transactions?
                .Where(p =>
                    p.TransactionDate >= firstDayOfFinancialYear)
                .Sum(p =>
                    p.ChargedAmount - p.PaidAmount - p.HousingBenefitAmount) ?? 0;
            // Sum of all the charges - Tenants
            var weeklyTotalCharges = charges?.DetailedCharges.Where(c =>
                c.Frequency.ToLower() == "weekly").Sum(c => c.Amount);

            var yearlyRentDebit = charges?.DetailedCharges.Where(c =>
                    c.Type.ToLower() == "rent" &&
                    c.Frequency.ToLower() == "weekly").Sum(c => c.Amount) * 52;

            var rent = charges?.DetailedCharges
                    .LastOrDefault(c => c.Type.ToLower() == "rent")?.Amount ?? 0;

            // Check for Tenants and Leaseholders
            decimal serviceCharge;
            if (isLeaseholder)
            {
                serviceCharge = charges?.DetailedCharges
                    .Where(c => c.Type.ToLower() == "service")?
                    .Sum(s => s.Amount) / 12 ?? 0;
            }
            else
            {
                serviceCharge = charges?.DetailedCharges
                    .Where(c => c.Type.ToLower() == "service")?
                    .Sum(s => s.Amount) ?? 0;
            }

            var paidThisYear = transactions?.Where(p =>
                p.TransactionDate >= firstDayOfFinancialYear &&
                p.TransactionDate <= lastDayOfFinancialYear).Sum(p => p.PaidAmount);

            return new PropertySummaryResponse
            {
                CurrentBalance = account?.ConsolidatedBalance,
                HousingBenefit = transactions?.Sum(s => s.HousingBenefitAmount) ?? 0,
                ServiceCharge = serviceCharge,
                TenancyType = tenure?.TenureType.Description,
                PersonTenureType = person == null ? null : tenure?.HouseholdMembers.FirstOrDefault(p => p.Id == person.Id)?.PersonTenureType,
                PrimaryTenantEmail = contacts?.Where(c =>
                        c.TargetType == TargetType.Person &&
                        c.ContactInformation.ContactType == ContactType.Email)
                    .Select(s =>
                        s.ContactInformation.Value).FirstOrDefault() ?? "",
                PrimaryTenantName = $"{person?.FirstName ?? ""} {person?.Surname ?? ""}",
                LeaseHolderName = $"{person?.FirstName ?? ""} {person?.Surname ?? ""}",
                PrimaryTenantPhoneNumber = contacts?.Where(c =>
                        c.TargetType == TargetType.Person &&
                        c.ContactInformation.ContactType == ContactType.Phone)
                    .Select(s =>
                        s.ContactInformation.Value).FirstOrDefault() ?? "",
                Rent = rent, //tenure?.Charges?.Rent,
                Address = asset?.AssetAddress,
                Prn = tenure?.PaymentReference,
                PropertyReference = tenure?.TenuredAsset?.PropertyReference,
                PropertySize = asset?.AssetCharacteristics?.NumberOfBedrooms ?? 0,
                TenancyStartDate = tenure?.StartOfTenureDate,
                YearToDate = yearToDate,
                WeeklyTotalCharges = weeklyTotalCharges,
                YearlyRentDebits = yearlyRentDebit,
                PaidThisYear = paidThisYear,
                ArrearsBalance = (account?.AccountBalance ?? 0) > 0 ? account?.AccountBalance : 0,
                AccountStartDate = account?.StartDate
            };
        }

        public static ResidentAssetsResponse ToResponse(Asset asset, TenureInformation tenure, List<Charge> charges, Account account)
        {
            var financialYear = DateTime.UtcNow.Year + ((DateTime.UtcNow.Month > 0 && DateTime.UtcNow.Month < 4) ? -1 : 0);
            var isLeaseHolder =
                tenure.TenureType?.Description == TenureTypes.LeaseholdRTB.Description
                || tenure.TenureType?.Description == TenureTypes.PrivateSaleLH.Description
                || tenure.TenureType?.Description == TenureTypes.SharedOwners.Description
                || tenure.TenureType?.Description == TenureTypes.SharedEquity.Description
                || tenure.TenureType?.Description == TenureTypes.ShortLifeLse.Description
                || tenure.TenureType?.Description == TenureTypes.LeaseholdStair.Description
                || tenure.TenureType?.Description == TenureTypes.FreeholdServ.Description;
            decimal rent;
            decimal serviceCharge;
            if (isLeaseHolder)
            {
                rent = 0;
                serviceCharge = charges?.Count == 0
                    ? 0
                    : charges?.FirstOrDefault(p => p.ChargeGroup == ChargeGroup.Leaseholders
                                                   && p.ChargeSubGroup == ChargeSubGroup.Estimate && p.ChargeYear == financialYear)
                        ?.DetailedCharges.Sum(x => x.Amount) ?? 0;

            }
            else
            {
                rent = charges?.Count == 0
                    ? 0
                    : charges?.Where(p => p.ChargeGroup == ChargeGroup.Tenants).OrderByDescending(x => x.ChargeYear).First()
                        ?.DetailedCharges.FirstOrDefault(x => x.Type.ToLower() == "rent")?.Amount ?? 0;
                serviceCharge = charges?.Count == 0
                    ? 0
                    : charges?.Where(p => p.ChargeGroup == ChargeGroup.Tenants).OrderByDescending(x => x.ChargeYear).First()
                        ?.DetailedCharges.Where(x => x.Type.ToLower() == "service")?.Sum(x => x.Amount) ?? 0;
            }

            return new ResidentAssetsResponse()
            {
                AssetAddress = asset?.AssetAddress,
                AssetType = asset?.AssetType,
                RentAccountNumber = tenure?.PaymentReference,
                CurrentBalance = account?.ConsolidatedBalance,
                RentCharge = rent,
                ServiceCharge = serviceCharge,
                Staircasting = -1,
                TenancyType = tenure?.TenureType,
                Tenure = new TenurePartialModel
                {
                    Id = tenure?.Id ?? Guid.Empty
                }
            };
        }

        public static PropertySummaryTenantsResponse ToResponse(Person person, TenureInformation tenants, List<ContactDetail> contacts)
        {
            if (person == null) throw new ArgumentNullException(nameof(person));
            if (tenants == null) throw new ArgumentNullException(nameof(tenants));

            return new PropertySummaryTenantsResponse()
            {
                FullAddress = tenants.TenuredAsset.FullAddress,
                Email = contacts?.Where(c =>
                        c.TargetType == TargetType.Person &&
                        c.ContactInformation.ContactType == ContactType.Email)
                    .Select(s => s.ContactInformation.Value).FirstOrDefault(),
                FullName = tenants.HouseholdMembers.FirstOrDefault(f => f.IsResponsible)?.FullName,
                StartDate = tenants.StartOfTenureDate,
                TenancyType = tenants.TenureType,
                PhoneNumber = contacts?.Where(c =>
                        c.TargetType == TargetType.Person &&
                        c.ContactInformation.ContactType == ContactType.Phone)
                    .Select(s => s.ContactInformation.Value).FirstOrDefault(),
                TenancyId = tenants.TenureType?.Code ?? "",
                TimeInPropertyY = (short) ((tenants.EndOfTenureDate ?? DateTime.UtcNow).Year - (tenants.StartOfTenureDate ?? DateTime.UtcNow).Year),
                TimeInPropertyM = (short) (((tenants.EndOfTenureDate ?? DateTime.UtcNow).Year - (tenants.StartOfTenureDate ?? DateTime.UtcNow).Year) * 12 +
                                  (tenants.EndOfTenureDate ?? DateTime.UtcNow).Month - (tenants.StartOfTenureDate ?? DateTime.UtcNow).Month),
                PersonType = person?.PersonTypes?.ToList(),
                TenureId = tenants.Id
            };
        }

        public static PropertyDetailsResponse ToResponse(Asset asset, Charge charge, bool isLeaseholder)
        {
            var weeklyCharges = isLeaseholder ? 0
                : charge?.DetailedCharges.Where(c =>
                    c.Frequency.ToLower() == "weekly").Sum(s => s.Amount) ?? 0;
            var monthlyCharge = isLeaseholder ? charge?.DetailedCharges.Where(c =>
                    c.Frequency.ToLower() == "yearly").Sum(s => s.Amount) / 12
                : weeklyCharges * 4;

            return new PropertyDetailsResponse()
            {
                Bedrooms = asset.AssetCharacteristics.NumberOfBedrooms,
                FullAddress = $"{asset.AssetAddress.AddressLine1} {asset.AssetAddress.AddressLine2} {asset.AssetAddress.AddressLine3} asset.AssetAddress.AddressLine4",
                PropertyValue = charge?.DetailedCharges?.FirstOrDefault(c => c.Type.ToLower() == "valuation")?.Amount,
                RentModel = null,
                The1999Value = charge?.DetailedCharges?.FirstOrDefault(c => c.Type.ToLower().Contains("1999"))?.Amount,
                ExtraCharges = charge?.DetailedCharges?.Where(w => w.Type.ToLower() != "valuation"
                                                               && w.Type.ToLower() != "rent"
                                                               && !w.Type.ToLower().Contains("1999")).Select(p => new ExtraCharge
                                                               {
                                                                   Name = p.SubType,
                                                                   Value = p.Amount
                                                               }).ToList(),
                WeeklyCharge = weeklyCharges,
                YearlyCharge = monthlyCharge * 12
            };
        }
    }
}
