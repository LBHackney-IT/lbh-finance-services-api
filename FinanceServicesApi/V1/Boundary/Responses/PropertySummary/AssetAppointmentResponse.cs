using System;
using System.Collections.Generic;

namespace FinanceServicesApi.V1.Boundary.Responses.PropertySummary
{
    public class AssetAppointmentResponse
    {
        public Guid AssetId { get; set; }

        public List<ChargesTotalResponse> EstateCostTotal { get; set; }
        public List<ChargesTotalResponse> BlockCostTotal { get; set; }
        public List<ChargesTotalResponse> PropertyCostTotal { get; set; }

        public string EstateName { get; set; }
        public List<PropertyCostTotals> EstateCosts { get; set; }

        public string BlockName { get; set; }
        public List<PropertyCostTotals> BlockCosts { get; set; }

        public List<PropertyCostTotals> PropertyCosts { get; set; }
    }
}
