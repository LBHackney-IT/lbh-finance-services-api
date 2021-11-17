using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FinanceServicesApi.V1.Domain
{
    public class AccountTenure
    {
        /// <example>
        ///     31245
        /// </example>
        [NotNull]
        public string TenancyId { get; set; }

        /// <example>
        ///     Introductory
        /// </example>
        [NotNull]
        public string TenancyType { get; set; }

        /// <example>
        ///     285 Avenue, 315 Amsterdam
        /// </example>
        [NotNull]
        public string FullAddress { get; set; }

        public IEnumerable<PrimaryTenants> PrimaryTenants { get; set; }
    }
}
