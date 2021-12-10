using System;

namespace FinanceServicesApi.V1.Boundary.Request
{
    public class ResidentSummaryRequest : BaseRequest
    {
        /// <summary>
        /// The owner of the account
        /// </summary>
        public Guid PersonId { get; set; }
    }
}
