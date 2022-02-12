using FinanceServicesApi.V1.Infrastructure.Enums;

namespace FinanceServicesApi.V1.Boundary.Responses.PropertySummary
{
    public class ChargesTotalResponse
    {
        ///<example>50.5</example>
        public decimal Amount { get; set; }

        ///<example>2021</example>
        public short Year { get; set; }

        ///<example>Actual</example>
        public ChargeSubGroup Type { get; set; }
    }
}
