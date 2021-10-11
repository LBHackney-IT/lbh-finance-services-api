using System;
using FinanceServicesApi.V1.Infrastructure;

namespace FinanceServicesApi.V1.Domain
{
    public class SuspenseResolutionInfo
    {
        [RequiredDateTime]
        public DateTime ResolutionDate { get; set; }

        public bool IsResolve => IsConfirmed && IsApproved;
        public bool IsConfirmed { get; set; } = false;
        public DateTime ConfirmedDate { get; set; }
        public bool IsApproved { get; set; } = false;
        public DateTime ApprovedDate { get; set; }
        public string Note { get; set; }
    }
}
