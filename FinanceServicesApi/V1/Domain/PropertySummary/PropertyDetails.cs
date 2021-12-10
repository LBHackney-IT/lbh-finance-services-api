using System;

namespace FinanceServicesApi.V1.Domain.PropertySummary
{
    public class PropertyDetails
    {
        public string Type { get; set; }
        public string SubType { get; set; }
        public string Frequency { get; set; }
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
