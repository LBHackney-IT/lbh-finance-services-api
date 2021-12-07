using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using FinanceServicesApi.V1.Infrastructure;
using Hackney.Shared.HousingSearch.Domain.Person;
using Hackney.Shared.HousingSearch.Domain.Transactions;

namespace FinanceServicesApi.V1.Domain
{
    public class Transaction
    {
        public Guid Id { get; set; }

        [AllowNull]
        public Guid TargetId { get; set; }

        [Range(1, 53)]
        public short PeriodNo { get; set; }

        public string TransactionSource { get; set; }

        [AllowedValues(typeof(TransactionType))]
        public TransactionType TransactionType { get; set; }

        [RequiredDateTime]
        public DateTime TransactionDate { get; set; }

        [Range(0.0, 999999999999)]
        public decimal TransactionAmount { get; set; }

        public string PaymentReference { get; set; }

        [StringLength(8, MinimumLength = 8, ErrorMessage = "The field BankAccountNumber must be a string with a length exactly equals to 8.")]
        public string BankAccountNumber { get; set; }

        [Required]
        public string SortCode { get; set; }

        public bool IsSuspense => TargetId == Guid.Empty;

        [Range(1, 12)]
        [Required]
        public int FinancialMonth { get; set; }

        [Range(1, int.MaxValue)]
        [Required]
        public int FinancialYear { get; set; }

        [Range(0.0, 999999999999)]
        public decimal PaidAmount { get; set; }

        [Range(0.0, 999999999999)]
        public decimal ChargedAmount { get; set; }

        public decimal BalanceAmount { get; set; }

        [Range(0.0, 999999999999)]
        public decimal HousingBenefitAmount { get; set; }

        public string Address { get; set; }

        public Person Person { get; set; }

        public string Fund { get; set; }

        [AllowNull]
        public SuspenseResolutionInfo SuspenseResolutionInfo { get; set; }
    }
}
