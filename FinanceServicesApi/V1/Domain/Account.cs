using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using FinanceServicesApi.V1.Infrastructure;

namespace FinanceServicesApi.V1.Domain
{
    public class Account
    {
        /// <summary>
        ///     Foreign reference number to attache to the the parent account.
        /// </summary>
        /// <example>
        ///     74c5fbc4-2fc8-40dc-896a-0cfa671fc435
        /// </example>
        public Guid ParentAccountId { get; set; }

        /// <example>
        ///     123234345
        /// </example>
        [Required]
        [NotNull]
        public string PaymentReference { get; set; }

        /// <example>
        ///     Estate
        /// </example>
        [AllowedValues(typeof(TargetType))]
        [Required]
        public TargetType TargetType { get; set; }

        /// <example>
        ///     74c5fbc4-2fc8-40dc-896a-0cfa671fc832
        /// </example>
        [NonEmptyGuid]
        public Guid TargetId { get; set; }

        /// <example>
        ///     Master
        /// </example>
        [AllowedValues(typeof(AccountType))]
        [Required]
        public AccountType AccountType { get; set; }

        /// <example>
        ///     MajorWorks
        /// </example>
        [AllowedValues(typeof(RentGroupType))]
        [Required]
        public RentGroupType RentGroupType { get; set; }

        /// <example>
        ///     Master Account
        /// </example>
        [Required]
        public string AgreementType { get; set; }

        /// <example>
        ///     2021-03-29T15:10:37.471Z
        /// </example>
        public DateTime StartDate { get; set; }

        /// <example>
        ///     2021-03-29T15:10:37.471Z
        /// </example>
        public DateTime EndDate { get; set; }

        /// <example>
        ///     Active
        /// </example>
        public AccountStatus AccountStatus { get; set; }
        /// <example>
        ///     74c5fbc4-2fc8-40dc-896a-0cfa671fc832
        /// </example>
        public Guid Id { get; set; }

        /// <example>
        ///     Admin
        /// </example>
        [Required]
        public string LastUpdatedBy { get; set; }

        /// <example>
        ///     123.01
        /// </example>
        public decimal AccountBalance { get; set; } = 0;

        /// <example>
        ///     278.05
        /// </example>
        public decimal ConsolidatedBalance { get; set; } = 0;

        [NotNull]
        public IEnumerable<ConsolidatedCharge> ConsolidatedCharges { get; set; }

        [NotNull]
        public AccountTenure Tenure { get; set; }
        /// <example>
        ///     2021-03-29T15:10:37.471Z
        /// </example>
        public DateTime CreatedAt { get; set; }

        /// <example>
        ///     2021-03-29T15:10:37.471Z
        /// </example>
        public DateTime LastUpdatedAt { get; set; }

        /// <example>
        ///     Admin
        /// </example>
        [Required]
        public string CreatedBy { get; set; }
    }
}
