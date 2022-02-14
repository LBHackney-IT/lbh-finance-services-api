using System.Collections.Generic;

namespace FinanceServicesApi.V1.Boundary.Responses.PropertySummary
{
    public class AssetApportionmentTotalsResponse
    {
        public List<ChargesTotalResponse> EstateCostTotal { get; set; } = new List<ChargesTotalResponse>();
        public List<ChargesTotalResponse> BlockCostTotal { get; set; } = new List<ChargesTotalResponse>();
        public List<ChargesTotalResponse> PropertyCostTotal { get; set; } = new List<ChargesTotalResponse>();
    }
}
