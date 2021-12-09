using System;

namespace FinanceServicesApi.V1.Boundary.Request
{
    public class ResidentSummaryRequest
    {
        /// <summary>
        /// The master account id which includes all relevant child accounts
        /// </summary>
        public Guid MasterAccountId { get; set; }
        /// <summary>
        /// The owner of the account's personId
        /// </summary>
        public Guid PersonId { get; set; }
    }
}
