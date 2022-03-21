using Amazon.SimpleNotificationService.Model;
using FinanceServicesApi.V1.Boundary.Responses;
using FinanceServicesApi.V1.Domain.Charges;
using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.UseCase.Interfaces;
using Hackney.Shared.Tenure.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Infrastructure.Exceptions;

namespace FinanceServicesApi.V1.UseCase
{
    public class GetYearlyRentDebitsUseCase : IGetYearlyRentDebitsUseCase
    {
        private readonly IAssetGateway _assetGateway;
        private readonly IChargesGateway _chargesGateway;
        private readonly ITenureInformationGateway _tenureInformationGateway;

        public GetYearlyRentDebitsUseCase(IAssetGateway assetGateway, IChargesGateway chargesGateway, ITenureInformationGateway tenureInformationGateway)
        {
            _assetGateway = assetGateway;
            _chargesGateway = chargesGateway;
            _tenureInformationGateway = tenureInformationGateway;
        }

        public async Task<List<YearlyRentDebitResponse>> ExecuteAsync(Guid assetId)
        {
            if (assetId == Guid.Empty)
                throw new ArgumentException($"{nameof(assetId)} shouldn't be empty.");

            var assetResponse = await _assetGateway.GetById(assetId).ConfigureAwait(false);
            if (assetResponse == null)
            {
                throw new AssetNotFoundException(assetId, "asset information");
            }

            var chargesResponse = await _chargesGateway.GetAllByAssetId(assetId).ConfigureAwait(false);
            var currentTenure = assetResponse.Tenure;

            var tenureInformationResponse = await _tenureInformationGateway.GetById(new Guid(currentTenure.Id)).ConfigureAwait(false);
            var leaseHolder = tenureInformationResponse?.HouseholdMembers.FirstOrDefault(p => p.IsResponsible);

            var yearlyRentDebitsResponse = new List<YearlyRentDebitResponse>();
            foreach (var charge in chargesResponse)
            {
                if (charge == null || !charge.DetailedCharges.Any()) continue;

                var rentCharges = charge.DetailedCharges.Where(c =>
                    c.Type.ToLower() == "rent").ToList();

                var serviceCharges = charge.DetailedCharges.Where(c =>
                    c.Type.ToLower() == "service").ToList();

                yearlyRentDebitsResponse.Add(new YearlyRentDebitResponse
                {
                    ChargeYear = charge?.ChargeYear ?? 0,
                    LeaseHolderName = leaseHolder?.FullName,
                    PaymentReferenceNumber = tenureInformationResponse?.PaymentReference,
                    RentCharge = rentCharges.Any() ? CalculateChargeTotal(rentCharges) : 0,
                    ServiceCharge = serviceCharges.Any() ? CalculateChargeTotal(serviceCharges) : 0
                });
            }

            return yearlyRentDebitsResponse;
        }

        private static decimal CalculateChargeTotal(IList<DetailedCharges> charges)
        {
            var totalAmount = 0m;
            foreach (var charge in charges)
            {
                // Get period. If end date not available default to 52 weeks
                var tenurePeriodInWeeks = 52m;
                if (charge == null) continue;
                if (charge.EndDate >= charge.StartDate)
                {
                    tenurePeriodInWeeks = ((charge.EndDate - charge.StartDate).Days) / 7m;
                }
                totalAmount += charge.Frequency.ToLower() switch
                {
                    "weekly" => charge.Amount * tenurePeriodInWeeks,
                    "monthly" => charge.Amount * (tenurePeriodInWeeks / 4m),
                    _ => charge.Amount * tenurePeriodInWeeks
                };
            }

            return decimal.Round(totalAmount, 2, MidpointRounding.AwayFromZero);
        }
    }
}
