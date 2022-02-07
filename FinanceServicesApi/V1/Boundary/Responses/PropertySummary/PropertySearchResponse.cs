using Hackney.Shared.Asset.Domain;
using System;

namespace FinanceServicesApi.V1.Boundary.Responses.PropertySummary
{
    public class PropertySearchResponse
    {
        public Guid AssetId { get; set; }

        public Guid TenureId { get; set; }

        public Guid? ChargeId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     15 Macron Court, Amhurst Rd, Hackney, London E8 1ND
        /// </example>
        public AssetAddress Address { get; set; }

        public int TotalEstimateAmount { get; set; }
    }
}
