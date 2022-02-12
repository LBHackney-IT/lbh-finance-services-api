using FinanceServicesApi.V1.Boundary.Responses.PropertySummary;
using FinanceServicesApi.V1.Infrastructure;
using FinanceServicesApi.V1.Infrastructure.Enums;
using FinanceServicesApi.V1.UseCase.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceServicesApi.V1.UseCase
{
    public class GetAssetAppointmentUseCase : IGetAssetAppointmentUseCase
    {
        private readonly IGetChargeByAssetIdUseCase _chargeUseCase;
        private List<short> _yearsToIterate;

        public GetAssetAppointmentUseCase(IGetChargeByAssetIdUseCase chargeUseCase)
        {
            _chargeUseCase = chargeUseCase;
        }

        public async Task<AssetAppointmentResponse> ExecuteAsync(Guid assetId, short startPeriodYear)
        {
            _yearsToIterate = Enumerable
                    .Range(startPeriodYear, DateTime.UtcNow.Year - startPeriodYear + 1)
                    .Select(year => (short) year)
                    .ToList();

            var allAssetCharges = await _chargeUseCase.ExecuteAsync(assetId).ConfigureAwait(false);
            if (allAssetCharges == null || allAssetCharges.Count == 0)
            {
                throw new ArgumentException($"No charges was loaded from Charges API for asset id: [{assetId}]");
            }

            var charges = allAssetCharges
                .Where(_ => _.ChargeYear >= startPeriodYear)
                .SelectMany(c => c.DetailedCharges.Select(dc => new DetailedChargeForYear(dc, c.ChargeYear, c.ChargeSubGroup)))
                .ToList();

            var assetAppointment = new AssetAppointmentResponse();
            assetAppointment.AssetId = assetId;
            assetAppointment.PropertyCosts = GetCostsGroupTotals(charges, ChargeType.Property);
            assetAppointment.PropertyCostTotal = GetCostsTotals(assetAppointment.PropertyCosts);

            assetAppointment.BlockName = "Marcon Court";
            assetAppointment.BlockCosts = GetCostsGroupTotals(charges, ChargeType.Block);
            assetAppointment.BlockCostTotal = GetCostsTotals(assetAppointment.BlockCosts);

            assetAppointment.EstateName = "Estate 1";
            assetAppointment.EstateCosts = GetCostsGroupTotals(charges, ChargeType.Estate);
            assetAppointment.EstateCostTotal = GetCostsTotals(assetAppointment.EstateCosts);

            return assetAppointment;
        }

        private List<PropertyCostTotals> GetCostsGroupTotals(
            List<DetailedChargeForYear> detailedCharges,
            ChargeType costsGroupType)
        {
            return detailedCharges
                .Where(_ => _.ChargeType == costsGroupType)
                .GroupBy(_ => _.SubType)
                .Select(_ => new PropertyCostTotals
                {
                    ChargeGroup = _.Key,
                    Totals = _yearsToIterate.Select(year => new ChargesTotalResponse()
                    {
                        Year = (short) year,
                        Type = GetTargetSubGroup(year),
                        Amount = _.Where(charge => charge.Year == year && charge.ChargeSubGroup == GetTargetSubGroup(year)).Sum(c => c.Amount)
                    }).ToList()
                }).ToList();
        }

        private List<ChargesTotalResponse> GetCostsTotals(List<PropertyCostTotals> propertyCostTotals)
        {
            return _yearsToIterate.Select(year => new ChargesTotalResponse()
            {
                Year = year,
                Type = GetTargetSubGroup(year),
                Amount = propertyCostTotals
                    .SelectMany(_ => _.Totals)
                    .Where(charge => charge.Year == year && charge.Type == GetTargetSubGroup(year))
                    .Sum(_ => _.Amount)
            }).ToList();
        }

        private static ChargeSubGroup GetTargetSubGroup(short year)
        {
            return year >= DateTime.UtcNow.Year - 1
                ? ChargeSubGroup.Estimate
                : ChargeSubGroup.Actual;
        }
    }
}
