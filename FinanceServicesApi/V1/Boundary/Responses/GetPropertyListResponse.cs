using FinanceServicesApi.V1.Boundary.Responses.PropertySummary;
using System.Collections.Generic;

namespace FinanceServicesApi.V1.Boundary.Responses
{
    public class GetPropertyListResponse
    {
        public long Total { get; set; }
        public List<PropertySearchResponse> Properties { get; set; }
    }
}
