using System;
using System.Collections.Generic;
using FinanceServicesApi.V1.Domain;

namespace FinanceServicesApi.V1.Boundary.Responses
{
    public class HousingSearchResponse<T> where T : class
    {
        public long Total { get; set; }
        public List<T> ResponseList { get; set; }
    }
}
