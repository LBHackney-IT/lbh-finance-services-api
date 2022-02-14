using FinanceServicesApi.V1.Boundary.Responses.PropertySummary;
using FinanceServicesApi.V1.Domain.Charges;
using FinanceServicesApi.V1.Infrastructure.Enums;

namespace FinanceServicesApi.V1.Infrastructure
{
    public class DetailedChargeForYear
    {
        public short Year { get; set; }

        public ChargeSubGroup? ChargeSubGroup { get; set; }

        public string Type { get; set; }

        public string SubType { get; set; }

        public ChargeType ChargeType { get; set; }

        public decimal Amount { get; set; }

        public DetailedChargeForYear(DetailedCharges detailedCharges, short year, ChargeSubGroup? chargeSubGroup)
        {
            Year = year;
            ChargeSubGroup = chargeSubGroup;
            Type = detailedCharges.Type;
            SubType = detailedCharges.SubType;
            ChargeType = detailedCharges.ChargeType;
            Amount = detailedCharges.Amount;
        }
    }
}
