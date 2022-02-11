using FinanceServicesApi.V1.Boundary.Responses.PropertySummary;
using FinanceServicesApi.V1.UseCase.Interfaces;
using System;
using System.Collections.Generic;
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

        public async Task<AssetAppointmentResponse> ExecuteAsync(Guid assetId, AssetType assetType)
        {
            var charges = await _chargeUseCase.ExecuteAsync(assetId).ConfigureAwait(false);

            if (charges == null || charges.Count == 0)
            {
                return null;
            }

            return new AssetAppointmentResponse
            {
                AssetId = assetId,
                AssetType = assetType,

                PropertyCosts = new List<PropertyCostTotals>
                {
                    new PropertyCostTotals
                    {
                        ChargeGroup = "Building insurance",
                        Totals = GenerateDummyTotals()
                    },
                    new PropertyCostTotals
                    {
                        ChargeGroup = "Ground rent",
                        Totals = GenerateDummyTotals()
                    }
                },
                PropertyCostTotal = GenerateDummyTotals(),

                BlockName = "Marcon Court",
                BlockCosts = new List<PropertyCostTotals>
                {
                    new PropertyCostTotals
                    {
                        ChargeGroup = "Block Repairs",
                        Totals = GenerateDummyTotals()
                    },
                    new PropertyCostTotals
                    {
                        ChargeGroup = "Block Cleaning",
                        Totals = GenerateDummyTotals()
                    },
                    new PropertyCostTotals
                    {
                        ChargeGroup = "Door Entry System",
                        Totals = GenerateDummyTotals()
                    }
                },
                BlockCostTotal = GenerateDummyTotals(),

                EstateName = "Estate 1",
                EstateCosts = new List<PropertyCostTotals>
                {
                    new PropertyCostTotals
                    {
                        ChargeGroup = "Estate Repairs",
                        Totals = GenerateDummyTotals()
                    },
                    new PropertyCostTotals
                    {
                        ChargeGroup = "Ground Maintenance",
                        Totals = GenerateDummyTotals()
                    },
                    new PropertyCostTotals
                    {
                        ChargeGroup = "Estate Repairs",
                        Totals = GenerateDummyTotals()
                    }
                },
                EstateCostTotal = GenerateDummyTotals(),

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
