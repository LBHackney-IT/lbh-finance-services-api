using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceServicesApi.V1.Boundary.Request
{
    public class ResidentSummaryRequest
    {
        public Guid MasterAccountId { get; set; }
        public Guid PersonId { get; set; }
    }
}
