using Hackney.Shared.Asset.Domain;
using System;
using System.Collections.Generic;

namespace FinanceServicesApi.V1.Boundary.Responses.PropertySummary
{
    public class AssetApportionmentResponse
    {
        ///<example>007b989f-a188-e187-1b2d-f8cb44cb8450</example>
        public Guid AssetId { get; set; }

        public AssetAddress AssetAddress { get; set; }

        public AssetApportionmentTotalsResponse Totals { get; set; } = new AssetApportionmentTotalsResponse();

        public List<PropertyCostTotals> EstateCosts { get; set; } = new List<PropertyCostTotals>();

        public List<PropertyCostTotals> BlockCosts { get; set; } = new List<PropertyCostTotals>();

        public List<PropertyCostTotals> PropertyCosts { get; set; } = new List<PropertyCostTotals>();
    }
}
