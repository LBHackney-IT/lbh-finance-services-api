using FinanceServicesApi.V1.Boundary.Request.Enums;
using FinanceServicesApi.V1.Boundary.Responses.PropertySummary;
using FinanceServicesApi.V1.Domain.Charges;
using FinanceServicesApi.V1.Gateways.Interfaces;
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
        private readonly IAssetGateway _assetGateway;

        public GetAssetApportionmentUseCase(IGetChargeByAssetIdUseCase chargeUseCase,
            IAssetGateway assetGateway)
        {
            _chargeUseCase = chargeUseCase;
            _assetGateway = assetGateway;
        }

        public async Task<AssetApportionmentResponse> ExecuteAsync(Guid assetId, short startPeriodYear, ChargeGroupFilter chargeGroupFilter)
        {
            var asset = await _assetGateway.GetById(assetId).ConfigureAwait(false);

            if (asset == null)
            {
                throw new ArgumentException($"No asset with id: [{assetId}]");
            }

            _yearsToIterate = Enumerable
                    .Range(startPeriodYear, DateTime.UtcNow.Year - startPeriodYear + 1)
                    .Select(year => (short) year)
                    .ToList();

            var allAssetCharges = await _chargeUseCase.ExecuteAsync(assetId).ConfigureAwait(false);
            if (allAssetCharges == null || allAssetCharges.Count == 0)
            {
                throw new ArgumentException($"No charges was loaded from Charges API for asset id: [{assetId}]");
            }

            var assetApportionment = new AssetApportionmentResponse();
            assetApportionment.AssetId = assetId;
            assetApportionment.AssetAddress = asset.AssetAddress;

            if (chargeGroupFilter == ChargeGroupFilter.Both || chargeGroupFilter == ChargeGroupFilter.Tenants)
            {
                assetApportionment.TenantApportionment = GenerateApportionmentForChargeGroup(allAssetCharges, startPeriodYear, ChargeGroup.Tenants);
                assetApportionment.TenantTotals = GetApportionmentTotal(assetApportionment.TenantApportionment);
            }

            if (chargeGroupFilter == ChargeGroupFilter.Both || chargeGroupFilter == ChargeGroupFilter.Leaseholders)
            {
                assetApportionment.LeaseholdApportionment = GenerateApportionmentForChargeGroup(allAssetCharges, startPeriodYear, ChargeGroup.Leaseholders);
                assetApportionment.LeaseholdTotals = GetApportionmentTotal(assetApportionment.LeaseholdApportionment);
            }

            return assetApportionment;
        }

        private ChargeGroupTotals GenerateApportionmentForChargeGroup(IEnumerable<Charge> allCharges,
            short startPeriodYear,
            ChargeGroup chargeGroup)
        {
            var apportionment = new ChargeGroupTotals();

            var detailedCharges = allCharges
              .Where(_ => _.ChargeYear >= startPeriodYear
                          && _.ChargeGroup == chargeGroup)
              .SelectMany(c => c.DetailedCharges.Select(dc => new DetailedChargeForYear(dc, c.ChargeYear, c.ChargeSubGroup)))
              .ToList();

            apportionment.PropertyCosts = GetCostsGroupTotals(detailedCharges, ChargeType.Property);

            apportionment.BlockCosts = GetCostsGroupTotals(detailedCharges, ChargeType.Block);

            apportionment.EstateCosts = GetCostsGroupTotals(detailedCharges, ChargeType.Estate);

            return apportionment;
        }

        private List<PropertyCostTotals> GetCostsGroupTotals(
            IEnumerable<DetailedChargeForYear> detailedCharges,
            ChargeType costsGroupType)
        {
            return detailedCharges
                .Where(_ => _.ChargeType == costsGroupType)
                .GroupBy(_ => _.SubType)
                .Select(_ => new PropertyCostTotals
                {
                    ChargeName = _.Key,
                    Totals = _yearsToIterate.Select(year => new ChargesTotalResponse()
                    {
                        Year = (short) year,
                        Type = GetTargetSubGroup(year),
                        Amount = _.Where(charge => charge.Year == year && charge.ChargeSubGroup == GetTargetSubGroup(year)).Sum(c => c.Amount)
                    }).ToList()
                }).ToList();
        }

        private AssetApportionmentTotalsResponse GetApportionmentTotal(ChargeGroupTotals chargeGroupTotals)
        {
            return new AssetApportionmentTotalsResponse
            {
                PropertyCostTotal = GetCostsTotals(chargeGroupTotals.PropertyCosts),
                BlockCostTotal = GetCostsTotals(chargeGroupTotals.BlockCosts),
                EstateCostTotal = GetCostsTotals(chargeGroupTotals.EstateCosts),
            };
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
