using Hackney.Shared.Asset.Domain;
using System;

namespace FinanceServicesApi.V1.Boundary.Responses.PropertySummary
{
    public class AssetApportionmentResponse
    {
        ///<example>007b989f-a188-e187-1b2d-f8cb44cb8450</example>
        public Guid AssetId { get; set; }

        public AssetAddress AssetAddress { get; set; }

        public AssetApportionmentTotalsResponse TenantTotals { get; set; }

        public AssetApportionmentTotalsResponse LeaseholdTotals { get; set; }

        public ChargeGroupTotals TenantApportionment { get; set; }

        public ChargeGroupTotals LeaseholdApportionment { get; set; }
    }
}
