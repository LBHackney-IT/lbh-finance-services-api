using System;
using BaseApi.V1.Infrastructure;

namespace BaseApi.V1.Domain
{
    public class SuspenseResolutionInfo
    {
        [RequiredDateTime]
        public DateTime ResolutionDate { get; set; }
        public bool IsResolve
        {
            get
            {
                if ((IsConfirmed && IsApproved))
                    return true;
                return false;
            }
        }
        public bool IsConfirmed { get; set; } = false;
        public DateTime ConfirmedDate { get; set; }
        public bool IsApproved { get; set; } = false;
        public DateTime ApprovedDate { get; set; }
        public string Note { get; set; }
    }
}
