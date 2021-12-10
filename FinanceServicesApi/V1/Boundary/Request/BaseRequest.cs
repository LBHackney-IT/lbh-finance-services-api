using System;

namespace FinanceServicesApi.V1.Boundary.Request
{
    public abstract class BaseRequest
    {
        /// <summary>
        /// The master account id which includes all relevant child accounts
        /// </summary>
        public Guid MasterAccountId { get; set; }
    }
}
