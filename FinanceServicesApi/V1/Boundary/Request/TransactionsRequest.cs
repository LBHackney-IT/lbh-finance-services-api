using System;
using FinanceServicesApi.V1.Boundary.Request.Interfaces;
using FinanceServicesApi.V1.Boundary.Request.MetaData;

namespace FinanceServicesApi.V1.Boundary.Request
{
    public class TransactionsRequest : HousingSearchRequest, ISearchByTargetId
    {
        public Guid TargetId { get; set; }
    }
}
