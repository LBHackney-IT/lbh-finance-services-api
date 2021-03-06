using System;

namespace FinanceServicesApi.V1.Boundary.Request
{
    public abstract class BaseRequest
    {
        /// <summary>
        /// The master account id which includes all relevant child accounts
        /// </summary>
        public Guid TenureId { get; set; }
        /// <summary>
        /// The owner of the account
        /// </summary>
        public Guid PersonId { get; set; }
    }
}
