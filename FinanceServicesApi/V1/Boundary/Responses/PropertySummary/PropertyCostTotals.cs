using System.Collections.Generic;

namespace FinanceServicesApi.V1.Boundary.Responses.PropertySummary
{
    public class PropertyCostTotals
    {
        ///<example>Block Cleaning</example>
        public string ChargeName { get; set; }

        public List<ChargesTotalResponse> Totals { get; set; }
    }
}
