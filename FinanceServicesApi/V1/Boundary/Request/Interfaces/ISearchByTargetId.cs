using System;

namespace FinanceServicesApi.V1.Boundary.Request.Interfaces
{
    interface ISearchByTargetId
    {
        public Guid TargetId { get; set; }
    }
}
