using AutoFixture;
using FinanceServicesApi.V1.Boundary.Request.Enums;
using FinanceServicesApi.V1.Boundary.Responses.PropertySummary;
using FinanceServicesApi.V1.Domain.Charges;
using FinanceServicesApi.V1.Infrastructure.Enums;
using System;
using System.Collections.Generic;

namespace FinanceServicesApi.Tests.V1.Helper
{
    public static class ApportionmentGeneratorHelper
    {
        private static Fixture _fixture = new Fixture();

        public static Charge CreateCharge(int year, ChargeGroup chargeGroup, ChargeSubGroup chargeSubGroup, ChargeType chargeType, string subType, decimal amount)
        {
            var detailerCharge = _fixture.Build<DetailedCharges>()
                .With(_ => _.SubType, subType)
                .With(_ => _.ChargeType, chargeType)
                .With(_ => _.Amount, amount)
                .CreateMany(1);

            return _fixture.Build<Charge>()
                .With(_ => _.ChargeGroup, chargeGroup)
                .With(_ => _.ChargeYear, year)
                .With(_ => _.ChargeSubGroup, chargeSubGroup)
                .With(_ => _.DetailedCharges, detailerCharge)
                .Create();
        }

        public static ChargesTotalResponse CreateTotal(decimal amount, int year, ChargeSubGroup chargeSubGroup)
        {
            return _fixture.Build<ChargesTotalResponse>()
                 .With(_ => _.Amount, amount)
                 .With(_ => _.Type, chargeSubGroup)
                 .With(_ => _.Year, year)
                 .Create();
        }

        public static List<Charge> CreateTestChargesData(ChargeGroupFilter chargeGroupFilter)
        {
            List<Charge> charges = new List<Charge>();

            if (chargeGroupFilter == ChargeGroupFilter.Both || chargeGroupFilter == ChargeGroupFilter.Leaseholders)
            {
                charges.AppentDataForChargeGroup(ChargeGroup.Leaseholders);
            }


            if (chargeGroupFilter == ChargeGroupFilter.Both || chargeGroupFilter == ChargeGroupFilter.Tenants)
            {
                charges.AppentDataForChargeGroup(ChargeGroup.Tenants);
            }

            return charges;
        }

        public static List<Charge> AppentDataForChargeGroup(this List<Charge> charges, ChargeGroup group)
        {
            charges.Add(CreateCharge(DateTime.Now.Year - 3, group, ChargeSubGroup.Estimate, ChargeType.Estate, "Estate Cleaning", 100));
            charges.Add(CreateCharge(DateTime.Now.Year - 3, group, ChargeSubGroup.Actual, ChargeType.Estate, "Estate Cleaning", 100));
            charges.Add(CreateCharge(DateTime.Now.Year - 3, group, ChargeSubGroup.Actual, ChargeType.Estate, "Estate Cleaning", 200));
            charges.Add(CreateCharge(DateTime.Now.Year - 1, group, ChargeSubGroup.Estimate, ChargeType.Estate, "Estate Cleaning", 300));
            charges.Add(CreateCharge(DateTime.Now.Year - 1, group, ChargeSubGroup.Estimate, ChargeType.Estate, "Estate Cleaning", 50));
            charges.Add(CreateCharge(DateTime.Now.Year, group, ChargeSubGroup.Estimate, ChargeType.Estate, "Estate Cleaning", 800));
            charges.Add(CreateCharge(DateTime.Now.Year - 2, group, ChargeSubGroup.Actual, ChargeType.Estate, "Grounds Maintenance", 50));
            charges.Add(CreateCharge(DateTime.Now.Year, group, ChargeSubGroup.Actual, ChargeType.Estate, "Grounds Maintenance", 50));
            charges.Add(CreateCharge(DateTime.Now.Year, group, ChargeSubGroup.Estimate, ChargeType.Estate, "Grounds Maintenance", 300));
            charges.Add(CreateCharge(DateTime.Now.Year, group, ChargeSubGroup.Estimate, ChargeType.Estate, "Grounds Maintenance", 150));
            charges.Add(CreateCharge(DateTime.Now.Year - 3, group, ChargeSubGroup.Estimate, ChargeType.Estate, "Ground rent", 300));
            charges.Add(CreateCharge(DateTime.Now.Year - 3, group, ChargeSubGroup.Actual, ChargeType.Estate, "Ground rent", 300));

            charges.Add(CreateCharge(DateTime.Now.Year - 3, group, ChargeSubGroup.Estimate, ChargeType.Block, "Block Cleaning", 100));
            charges.Add(CreateCharge(DateTime.Now.Year - 4, group, ChargeSubGroup.Actual, ChargeType.Block, "Block Cleaning", 200));
            charges.Add(CreateCharge(DateTime.Now.Year - 3, group, ChargeSubGroup.Actual, ChargeType.Block, "Block Cleaning", 100));
            charges.Add(CreateCharge(DateTime.Now.Year - 1, group, ChargeSubGroup.Estimate, ChargeType.Block, "Grounds Maintenance", 50));
            charges.Add(CreateCharge(DateTime.Now.Year - 1, group, ChargeSubGroup.Estimate, ChargeType.Block, "Grounds Maintenance", 50));
            charges.Add(CreateCharge(DateTime.Now.Year, group, ChargeSubGroup.Estimate, ChargeType.Block, "Heating Fuel", 500));
            charges.Add(CreateCharge(DateTime.Now.Year, group, ChargeSubGroup.Estimate, ChargeType.Block, "Heating Fuel", 100));
            charges.Add(CreateCharge(DateTime.Now.Year, group, ChargeSubGroup.Estimate, ChargeType.Block, "Heating Fuel", 50));
            charges.Add(CreateCharge(DateTime.Now.Year, group, ChargeSubGroup.Actual, ChargeType.Block, "Heating Fuel", 50));

            charges.Add(CreateCharge(DateTime.Now.Year - 3, group, ChargeSubGroup.Estimate, ChargeType.Property, "Building insurance", 100));
            charges.Add(CreateCharge(DateTime.Now.Year - 3, group, ChargeSubGroup.Actual, ChargeType.Property, "Building insurance", 100));
            charges.Add(CreateCharge(DateTime.Now.Year - 3, group, ChargeSubGroup.Actual, ChargeType.Property, "Building insurance", 50));
            charges.Add(CreateCharge(DateTime.Now.Year - 1, group, ChargeSubGroup.Estimate, ChargeType.Property, "Building insurance", 200));
            charges.Add(CreateCharge(DateTime.Now.Year - 1, group, ChargeSubGroup.Estimate, ChargeType.Property, "Building insurance", 100));
            charges.Add(CreateCharge(DateTime.Now.Year - 1, group, ChargeSubGroup.Actual, ChargeType.Property, "Building insurance", 100));
            charges.Add(CreateCharge(DateTime.Now.Year, group, ChargeSubGroup.Estimate, ChargeType.Property, "Building insurance", 100));
            charges.Add(CreateCharge(DateTime.Now.Year, group, ChargeSubGroup.Estimate, ChargeType.Property, "Building insurance", 50));
            charges.Add(CreateCharge(DateTime.Now.Year, group, ChargeSubGroup.Actual, ChargeType.Property, "Building insurance", 50));
            charges.Add(CreateCharge(DateTime.Now.Year - 3, group, ChargeSubGroup.Actual, ChargeType.Property, "Management Fee", 200));
            charges.Add(CreateCharge(DateTime.Now.Year - 4, group, ChargeSubGroup.Actual, ChargeType.Property, "Management Fee", 200));
            charges.Add(CreateCharge(DateTime.Now.Year - 2, group, ChargeSubGroup.Actual, ChargeType.Property, "Management Fee", 200));
            charges.Add(CreateCharge(DateTime.Now.Year - 2, group, ChargeSubGroup.Actual, ChargeType.Property, "Management Fee", 800));
            charges.Add(CreateCharge(DateTime.Now.Year - 2, group, ChargeSubGroup.Estimate, ChargeType.Property, "Management Fee", 200));
            charges.Add(CreateCharge(DateTime.Now.Year - 1, group, ChargeSubGroup.Actual, ChargeType.Property, "Management Fee", 200));
            charges.Add(CreateCharge(DateTime.Now.Year - 1, group, ChargeSubGroup.Estimate, ChargeType.Property, "Management Fee", 150));
            charges.Add(CreateCharge(DateTime.Now.Year - 1, group, ChargeSubGroup.Estimate, ChargeType.Property, "Management Fee", 300));
            charges.Add(CreateCharge(DateTime.Now.Year, group, ChargeSubGroup.Estimate, ChargeType.Property, "Management Fee", 300));
            charges.Add(CreateCharge(DateTime.Now.Year, group, ChargeSubGroup.Estimate, ChargeType.Property, "Management Fee", 50));
            charges.Add(CreateCharge(DateTime.Now.Year, group, ChargeSubGroup.Actual, ChargeType.Property, "Management Fee", 50));

            return charges;
        }

        public static AssetApportionmentTotalsResponse CalculateDefaultApportionmentTotals()
        {
            return new AssetApportionmentTotalsResponse
            {
                EstateCostTotal = new List<ChargesTotalResponse>
                {
                    CreateTotal(1250, DateTime.Now.Year, ChargeSubGroup.Estimate),
                    CreateTotal(350, DateTime.Now.Year - 1, ChargeSubGroup.Estimate),
                    CreateTotal(50, DateTime.Now.Year - 2, ChargeSubGroup.Actual),
                    CreateTotal(600, DateTime.Now.Year - 3, ChargeSubGroup.Actual)
                },
                BlockCostTotal = new List<ChargesTotalResponse>
                {
                    CreateTotal(650, DateTime.Now.Year, ChargeSubGroup.Estimate),
                    CreateTotal(100, DateTime.Now.Year - 1, ChargeSubGroup.Estimate),
                    CreateTotal(0, DateTime.Now.Year - 2, ChargeSubGroup.Actual),
                    CreateTotal(100, DateTime.Now.Year - 3, ChargeSubGroup.Actual)
                },
                PropertyCostTotal = new List<ChargesTotalResponse>
                {
                    CreateTotal(500, DateTime.Now.Year, ChargeSubGroup.Estimate),
                    CreateTotal(750, DateTime.Now.Year - 1, ChargeSubGroup.Estimate),
                    CreateTotal(1000, DateTime.Now.Year - 2, ChargeSubGroup.Actual),
                    CreateTotal(350, DateTime.Now.Year - 3, ChargeSubGroup.Actual)
                }
            };
        }

        public static ChargeGroupTotals CalculateDefaultApportionment()
        {
            return new ChargeGroupTotals
            {
                EstateCosts = new List<PropertyCostTotals>
                {
                    new PropertyCostTotals()
                    {
                        ChargeName = "Estate Cleaning",
                        Totals = new List<ChargesTotalResponse>
                        {
                            CreateTotal(800, DateTime.Now.Year, ChargeSubGroup.Estimate),
                            CreateTotal(350, DateTime.Now.Year - 1, ChargeSubGroup.Estimate),
                            CreateTotal(0, DateTime.Now.Year - 2, ChargeSubGroup.Actual),
                            CreateTotal(300, DateTime.Now.Year - 3, ChargeSubGroup.Actual)
                        }
                    },
                    new PropertyCostTotals()
                    {
                        ChargeName = "Grounds Maintenance",
                        Totals = new List<ChargesTotalResponse>
                        {
                            CreateTotal(450, DateTime.Now.Year, ChargeSubGroup.Estimate),
                            CreateTotal(0, DateTime.Now.Year - 1, ChargeSubGroup.Estimate),
                            CreateTotal(50, DateTime.Now.Year - 2, ChargeSubGroup.Actual),
                            CreateTotal(0, DateTime.Now.Year - 3, ChargeSubGroup.Actual)
                        }
                    },
                    new PropertyCostTotals()
                    {
                        ChargeName = "Ground rent",
                        Totals = new List<ChargesTotalResponse>
                        {
                            CreateTotal(0, DateTime.Now.Year, ChargeSubGroup.Estimate),
                            CreateTotal(0, DateTime.Now.Year - 1, ChargeSubGroup.Estimate),
                            CreateTotal(0, DateTime.Now.Year - 2, ChargeSubGroup.Actual),
                            CreateTotal(300, DateTime.Now.Year - 3, ChargeSubGroup.Actual)
                        }
                    }
                },
                BlockCosts = new List<PropertyCostTotals>
                {
                    new PropertyCostTotals()
                    {
                        ChargeName = "Block Cleaning",
                        Totals = new List<ChargesTotalResponse>()
                        {
                            CreateTotal(0, DateTime.Now.Year, ChargeSubGroup.Estimate),
                            CreateTotal(0, DateTime.Now.Year - 1, ChargeSubGroup.Estimate),
                            CreateTotal(0, DateTime.Now.Year - 2, ChargeSubGroup.Actual),
                            CreateTotal(100, DateTime.Now.Year - 3, ChargeSubGroup.Actual)
                        },

                    },
                    new PropertyCostTotals()
                    {
                        ChargeName = "Grounds Maintenance",
                        Totals = new List<ChargesTotalResponse>()
                        {
                            CreateTotal(0, DateTime.Now.Year, ChargeSubGroup.Estimate),
                            CreateTotal(100, DateTime.Now.Year - 1, ChargeSubGroup.Estimate),
                            CreateTotal(0, DateTime.Now.Year - 2, ChargeSubGroup.Actual),
                            CreateTotal(0, DateTime.Now.Year - 3, ChargeSubGroup.Actual)
                        }
                    },
                    new PropertyCostTotals()
                    {
                        ChargeName = "Heating Fuel",
                        Totals = new List<ChargesTotalResponse>()
                        {
                            CreateTotal(650, DateTime.Now.Year, ChargeSubGroup.Estimate),
                            CreateTotal(0, DateTime.Now.Year - 1, ChargeSubGroup.Estimate),
                            CreateTotal(0, DateTime.Now.Year - 2, ChargeSubGroup.Actual),
                            CreateTotal(0, DateTime.Now.Year - 3, ChargeSubGroup.Actual)
                        }
                    }
                },
                PropertyCosts = new List<PropertyCostTotals>
                {
                    new PropertyCostTotals
                    {
                        ChargeName = "Building insurance",
                        Totals = new List<ChargesTotalResponse>()
                        {
                            CreateTotal(150, DateTime.Now.Year, ChargeSubGroup.Estimate),
                            CreateTotal(300, DateTime.Now.Year - 1, ChargeSubGroup.Estimate),
                            CreateTotal(0, DateTime.Now.Year - 2, ChargeSubGroup.Actual),
                            CreateTotal(150, DateTime.Now.Year - 3, ChargeSubGroup.Actual)
                        }
                    },
                    new PropertyCostTotals
                    {
                        ChargeName = "Management Fee",
                        Totals = new List<ChargesTotalResponse>()
                        {
                            CreateTotal(350, DateTime.Now.Year, ChargeSubGroup.Estimate),
                            CreateTotal(450, DateTime.Now.Year - 1, ChargeSubGroup.Estimate),
                            CreateTotal(1000, DateTime.Now.Year - 2, ChargeSubGroup.Actual),
                            CreateTotal(200, DateTime.Now.Year - 3, ChargeSubGroup.Actual)
                        }
                    }
                }
            };
        }
    }
}
