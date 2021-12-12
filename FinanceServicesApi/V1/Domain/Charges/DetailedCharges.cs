using System;
using System.ComponentModel.DataAnnotations;
using FinanceServicesApi.V1.Infrastructure;
using FinanceServicesApi.V1.Infrastructure.Enums;

namespace FinanceServicesApi.V1.Domain.Charges
{
    public class DetailedCharges
    {
        [Required]
        public string Type { get; set; }

        [Required]
        public string SubType { get; set; }

        public ChargeType ChargeType { get; set; }

        public string ChargeCode { get; set; }

        [Required]
        public string Frequency { get; set; }

        [GreatAndEqualThan("0")]
        public decimal Amount { get; set; }

        [RequiredDateTime]
        public DateTime StartDate { get; set; }

        [RequiredDateTime]
        public DateTime EndDate { get; set; }
    }
}
