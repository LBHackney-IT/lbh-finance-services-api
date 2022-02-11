using FinanceServicesApi.V1.Boundary.Responses.PropertySummary;
using FinanceServicesApi.V1.Domain.Charges;
using FinanceServicesApi.V1.UseCase.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceServicesApi.V1.UseCase
{
    public class GetChargesSummaryByTypeUseCase : IGetChargesSummaryByTypeUseCase
    {
        private readonly IGetChargeByAssetIdUseCase _chargeUseCase;

        public GetChargesSummaryByTypeUseCase(IGetChargeByAssetIdUseCase chargeUseCase)
        {
            _chargeUseCase = chargeUseCase;
        }

        private static IEnumerable<PropertyCostTotals> GetPropertyCosts(IEnumerable<DetailedCharges> detailedCharges)
            => detailedCharges.Select(d => new PropertyCostTotals
            {
                ChargeGroup = d.SubType
            });

        public async Task<AssetAppointmentResponse> ExecuteAsync(Guid assetId, AssetType assetType)
        {
            var charges = await _chargeUseCase.ExecuteAsync(assetId).ConfigureAwait(false);

            if (charges == null || charges.Count == 0)
            {
                throw new ArgumentException($"{nameof(charges)} is empty"); ;
            }

            var chargeDetails = charges.SelectMany(c => c.DetailedCharges);

            var blocksDetails = chargeDetails.Where(d => d.ChargeType == Infrastructure.Enums.ChargeType.Block);
            var propsDetails = chargeDetails.Where(d => d.ChargeType == Infrastructure.Enums.ChargeType.Property);
            var estsatesDetails = chargeDetails.Where(d => d.ChargeType == Infrastructure.Enums.ChargeType.Estate);

            return new AssetAppointmentResponse
            {
                AssetId = assetId,
                AssetType = assetType,

                PropertyCosts = GetPropertyCosts(propsDetails).ToList(),
                PropertyCostTotal = GenerateDummyTotals(),

                BlockName = "Marcon Court",
                BlockCosts = GetPropertyCosts(blocksDetails).ToList(),
                BlockCostTotal = GenerateDummyTotals(),

                EstateName = "Estate 1",
                EstateCosts = GetPropertyCosts(estsatesDetails).ToList(),
            };
        }

        private static List<ChargesTotalResponse> GenerateDummyTotals()
        {
            var random = new Random();

            return new List<ChargesTotalResponse>
            {
                new ChargesTotalResponse
                {
                    Year = 2020,
                    Type = ChargeSubGroup.Actual,
                    Amount = (decimal)random.NextDouble() * random.Next(1000)
                },
                new ChargesTotalResponse
                {
                    Year = 2021,
                    Type = ChargeSubGroup.Estimate,
                    Amount = (decimal)random.NextDouble() * random.Next(1000)
                },
                new ChargesTotalResponse
                {
                    Year = 2022,
                    Type = ChargeSubGroup.Estimate,
                    Amount = (decimal)random.NextDouble() * random.Next(1000)
                }
            };
        }
    }
}
