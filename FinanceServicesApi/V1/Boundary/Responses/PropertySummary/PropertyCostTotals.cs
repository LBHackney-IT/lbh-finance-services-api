using System.Collections.Generic;

namespace FinanceServicesApi.V1.Boundary.Responses.PropertySummary
{
    public class PropertyCostTotals
    {
        public string ChargeGroup { get; set; }

        public List<ChargesTotalResponse> Totals { get; set; }
    }
}
