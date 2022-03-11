using System.Collections.Generic;

namespace FinanceServicesApi.V1.Boundary.Responses.PropertySummary
{
    public class ChargeGroupTotals
    {
        public List<PropertyCostTotals> EstateCosts { get; set; } = new List<PropertyCostTotals>();

        public List<PropertyCostTotals> BlockCosts { get; set; } = new List<PropertyCostTotals>();

        public List<PropertyCostTotals> PropertyCosts { get; set; } = new List<PropertyCostTotals>();
    }
}
