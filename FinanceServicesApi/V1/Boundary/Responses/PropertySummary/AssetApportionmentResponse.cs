using System;
using System.Collections.Generic;

namespace FinanceServicesApi.V1.Boundary.Responses.PropertySummary
{
    public class AssetApportionmentResponse
    {
        ///<example>007b989f-a188-e187-1b2d-f8cb44cb8450</example>
        public Guid AssetId { get; set; }

        public List<ChargesTotalResponse> EstateCostTotal { get; set; }
        public List<ChargesTotalResponse> BlockCostTotal { get; set; }
        public List<ChargesTotalResponse> PropertyCostTotal { get; set; }

        ///<example>Estate 1</example>
        public string EstateName { get; set; }
        public List<PropertyCostTotals> EstateCosts { get; set; }

        ///<example>Marcon Court</example>
        public string BlockName { get; set; }
        public List<PropertyCostTotals> BlockCosts { get; set; }

        public List<PropertyCostTotals> PropertyCosts { get; set; }
    }
}
