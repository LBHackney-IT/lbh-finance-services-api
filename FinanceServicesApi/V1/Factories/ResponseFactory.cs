using System;
using System.Collections.Generic;
using System.Linq;
using FinanceServicesApi.V1.Boundary.Responses;
using FinanceServicesApi.V1.Boundary.Responses.PropertySummary;
using FinanceServicesApi.V1.Boundary.Responses.ResidentSummary;
using FinanceServicesApi.V1.Domain.AccountModels;
using FinanceServicesApi.V1.Domain.Charges;
using FinanceServicesApi.V1.Domain.ContactDetails;
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
            var arrearsAfterPayment = (account?.AccountBalance ?? 0) - (transaction?.TransactionAmount ?? 0);
            return new ConfirmTransferResponse
            {
                Address = transaction?.Address,
                ArrearsAfterPayment = arrearsAfterPayment < 0 ? 0 : arrearsAfterPayment,
                CurrentArrears = account?.AccountBalance ?? 0,
                Payee = transaction?.Person?.FullName,
                RentAccountNumber = account?.PaymentReference,
                Resident = account?.Tenure?.PrimaryTenants?.FirstOrDefault()?.FullName
            };
        }

        public static ResidentSummaryResponse ToResponse(Person person,
            TenureInformation tenure,
            Account account,
            List<Domain.Charges.Charge> charges,
            List<ContactDetail> contacts,
            List<Transaction> transactions)
        {
            return new ResidentSummaryResponse
            {
                CurrentBalance = account?.ConsolidatedBalance,
                HousingBenefit = transactions?.Sum(s => s.HousingBenefitAmount),
                ServiceCharge = charges?.Count == 0 ? 0 : charges?.Sum(p => p.DetailedCharges.Where(c => c.Type.ToLower() == "service").Sum(c => c.Amount)),
                DateOfBirth = person?.DateOfBirth,
                PersonId = "-",
                TenureId = account?.Tenure?.TenureId,
                LastPaymentAmount = transactions?.Count == 0 ? 0 : transactions?.Last().PaidAmount,
                LastPaymentDate = transactions?.Count == 0 ? (DateTime?) null : transactions?.Last(p => p.PaidAmount > 0).TransactionDate,
                PrimaryTenantAddress = tenure?.TenuredAsset?.FullAddress,
                TenureType = tenure?.TenureType.Code,
                PrimaryTenantEmail = contacts.Where(c => c.TargetType == TargetType.Person && c.ContactInformation.ContactType == ContactType.Email)
                    .Select(s => s.ContactInformation.Value).FirstOrDefault() ?? "",
                PrimaryTenantName = $"{person?.FirstName ?? ""} {person?.Surname ?? ""}",
                PrimaryTenantPhoneNumber = contacts.Where(c => c.TargetType == TargetType.Person && c.ContactInformation.ContactType == ContactType.Phone)
                    .Select(s => s.ContactInformation.Value).FirstOrDefault() ?? "",
                TenureStartDate = tenure?.StartOfTenureDate,
                WeeklyTotalCharges = charges?.Sum(p =>
                    p.DetailedCharges.Where(c =>
                        c.Frequency.ToLower() == "weekly" &&
                        c.Type.ToLower() == "service").Sum(c => c.Amount))
            };
        }

        public static PropertySummaryResponse ToResponse(TenureInformation tenure,
            Person person,
            Account account,
            List<Charge> charges,
            List<ContactDetail> contacts,
            List<Transaction> transactions,
            Asset asset)
        {
            var firstMondayOfApril = new DateTime(DateTime.UtcNow.Year, 1, 1);
            while (firstMondayOfApril.DayOfWeek != DayOfWeek.Monday)
            {
                firstMondayOfApril = firstMondayOfApril.AddDays(1);
            }

            var ytd = transactions
                .Where(p =>
                    p.TransactionDate >= firstMondayOfApril)
                .Sum(p =>
                    p.ChargedAmount - p.PaidAmount - p.HousingBenefitAmount);
            var wtc = charges?.Sum(p =>
                p.DetailedCharges.Where(c =>
                    c.EndDate >= DateTime.UtcNow &&
                    c.Type.ToLower() == "service" &&
                    c.Frequency.ToLower() == "weekly").Sum(c => c.Amount));
            var yrd = charges?.Sum(p =>
                p.DetailedCharges.Where(c =>
                    c.EndDate >= DateTime.UtcNow &&
                    c.Type.ToLower() == "rent" &&
                    c.Frequency.ToLower() == "weekly").Sum(c => c.Amount)) * 52;
            var pty = transactions?.Where(p => p.TransactionDate.Year >= DateTime.UtcNow.Year).Sum(p => p.PaidAmount);
            return new PropertySummaryResponse
            {
                CurrentBalance = tenure?.Charges?.CurrentBalance,
                HousingBenefit = transactions.Sum(s => s.HousingBenefitAmount),
                ServiceCharge = charges?.Count == 0 ? 0 :
                    charges?.Sum(p =>
                        p.DetailedCharges.Where(c =>
                            c.Type.ToLower() == "service").
                            Sum(c => c.Amount)),
                TenancyType = tenure?.TenureType.Code,
                PersonTenureType = person == null ? null : tenure?.HouseholdMembers.First(p => p.Id == person.Id)?.PersonTenureType,
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
                Rent = tenure?.Charges?.Rent,
                Address = asset?.AssetAddress,
                Prn = tenure?.PaymentReference,
                PropertyReference = tenure?.TenuredAsset?.PropertyReference,
                PropertySize = asset?.AssetCharacteristics?.NumberOfBedrooms ?? 0,
                TenancyStartDate = tenure?.StartOfTenureDate,
                YearToDate = ytd,
                WeeklyTotalCharges = wtc,
                YearlyRentDebits = yrd,
                PaidThisYear = pty,
                ArrearsBalance = (account?.AccountBalance ?? 0) > 0 ? account?.AccountBalance : 0,
                AccountStartDate = account?.StartDate
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
                FullName = tenants.HouseholdMembers.First(f => f.IsResponsible)?.FullName,
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

        public static PropertyDetailsResponse ToResponse(Asset asset, List<Charge> charges)
        {
            List<DetailedCharges> chargesList = new List<DetailedCharges>();
            charges.ForEach(p =>
            {
                chargesList.AddRange(p.DetailedCharges);
            });

            var weeklyCharges = chargesList.Where(c =>
                c.EndDate >= DateTime.UtcNow &&
                c.Type.ToLower() == "service" &&
                c.Frequency.ToLower() == "weekly").Sum(s => s.Amount);

            return new PropertyDetailsResponse()
            {
                Bedrooms = asset.AssetCharacteristics.NumberOfBedrooms,
                FullAddress = $"{asset.AssetAddress.AddressLine1} {asset.AssetAddress.AddressLine2} {asset.AssetAddress.AddressLine3} asset.AssetAddress.AddressLine4",
                PropertyValue = chargesList?.FirstOrDefault(c => c.Type.ToLower() == "valuation")?.Amount,
                RentModel = null,
                The1999Value = chargesList?.FirstOrDefault(c => c.Type.ToLower().Contains("1999"))?.Amount,
                ExtraCharges = chargesList.ToList().Where(w => w.Type.ToLower() == "valuation" &&
                                                              w.Type.ToLower().Contains("1999")).Select(p => new ExtraCharge
                                                              {
                                                                  Name = p.Type,
                                                                  Value = p.Amount
                                                              }).ToList(),
                WeeklyCharge = weeklyCharges,
                YearlyCharge = weeklyCharges * 52
            };
        }
    }
}
