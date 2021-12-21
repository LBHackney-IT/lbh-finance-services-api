using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using FinanceServicesApi.V1.Infrastructure;
using FinanceServicesApi.V1.Infrastructure.Enums;

namespace FinanceServicesApi.V1.Domain.TransactionModels
{
    public class Transaction
    {
        public Guid Id { get; set; }

        [AllowNull]
        public Guid TargetId { get; set; }
        public TargetType TargetType { get; set; }
        [Range(1, 53)]
        public short PeriodNo { get; set; }

        public string TransactionSource { get; set; }

        [AllowedValues(typeof(TransactionType))]
        public TransactionType TransactionType { get; set; }

        [RequiredDateTime]
        public DateTime TransactionDate { get; set; }

        [GreatAndEqualThan("0.0")]
        public decimal TransactionAmount { get; set; }

        public string PaymentReference { get; set; }

        [StringLength(8, MinimumLength = 8, ErrorMessage = "The field BankAccountNumber must be a string with a length exactly equals to 8.")]
        public string BankAccountNumber { get; set; }

        [Required]
        public string SortCode { get; set; }

        public bool IsSuspense => TargetId == Guid.Empty;

        [Range(1, 12)]
        [Required]
        public short FinancialMonth { get; set; }

        [Range(1, short.MaxValue)]
        [Required]
        public short FinancialYear { get; set; }

        [GreatAndEqualThan("0.0")]
        public decimal PaidAmount { get; set; }

        [GreatAndEqualThan("0.0")]
        public decimal ChargedAmount { get; set; }

        public decimal BalanceAmount { get; set; }

        [GreatAndEqualThan("0.0")]
        public decimal HousingBenefitAmount { get; set; }

        public string Address { get; set; }

        public TransactionPerson Person { get; set; }

        [AllowNull]
        public string Fund { get; set; }

        [AllowNull]
        public SuspenseResolutionInfo SuspenseResolutionInfo { get; set; }
        public string LastUpdatedBy { get; set; }

        public DateTime LastUpdatedAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }
    }
}
