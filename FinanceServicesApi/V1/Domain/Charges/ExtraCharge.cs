using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using FinanceServicesApi.V1.Infrastructure;

namespace FinanceServicesApi.V1.Domain.Charges
{
    public class ExtraCharge
    {
        [Required]
        [NotNull]
        public string Name { get; set; }
        [GreatAndEqualThan("0")]
        public decimal? Value { get; set; }
    }
}
