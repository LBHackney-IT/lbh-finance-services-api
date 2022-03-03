using Hackney.Shared.Asset.Domain;
using System;
using System.Collections.Generic;

namespace FinanceServicesApi.V1.Boundary.Responses.PropertySummary
{
    public class PropertySearchResponse
    {
        public Guid AssetId { get; set; }

        public Guid? TenureId { get; set; }

        /// <example>
        ///     15 Macron Court, Amhurst Rd, Hackney, London E8 1ND
        /// </example>
        public AssetAddress Address { get; set; }

        public List<ChargesTotalResponse> Totals { get; set; } = new List<ChargesTotalResponse> { };
    }
}
