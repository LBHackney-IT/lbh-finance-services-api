using System.Collections.Generic;

namespace FinanceServicesApi.V1.Domain.AccountModels
{
    public class AccountTenureSubSet
    {
        /// <example>
        ///     31245
        /// </example>
        public string TenureId { get; set; }

        public TenureType TenureType { get; set; }

        /// <example>
        ///     285 Avenue, 315 Amsterdam
        /// </example>
        public string FullAddress { get; set; }

        public List<PrimaryTenants> PrimaryTenants { get; set; }
    }
}
