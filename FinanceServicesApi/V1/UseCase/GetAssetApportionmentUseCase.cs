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
    public class GetAssetApportionmentUseCase : IGetAssetApportionmentUseCase
    {
        private readonly IGetChargeByAssetIdUseCase _chargeUseCase;
        private List<short> _yearsToIterate;

        public GetAssetApportionmentUseCase(IGetChargeByAssetIdUseCase chargeUseCase)
        {
            _chargeUseCase = chargeUseCase;
        }

        public async Task<AssetApportionmentResponse> ExecuteAsync(Guid assetId, short startPeriodYear)
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

            var assetApportionment = new AssetApportionmentResponse();
            assetApportionment.AssetId = assetId;
            assetApportionment.PropertyCosts = GetCostsGroupTotals(charges, ChargeType.Property);
            assetApportionment.PropertyCostTotal = GetCostsTotals(assetApportionment.PropertyCosts);

            assetApportionment.BlockName = "Marcon Court";
            assetApportionment.BlockCosts = GetCostsGroupTotals(charges, ChargeType.Block);
            assetApportionment.BlockCostTotal = GetCostsTotals(assetApportionment.BlockCosts);

            assetApportionment.EstateName = "Estate 1";
            assetApportionment.EstateCosts = GetCostsGroupTotals(charges, ChargeType.Estate);
            assetApportionment.EstateCostTotal = GetCostsTotals(assetApportionment.EstateCosts);

            return assetApportionment;
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
