using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceServicesApi.V1.Boundary.Request
{
    public class PropertySummaryRequest : BaseRequest
    {
        /// <summary>
        ///     The tenured asset in the provided account
        /// </summary>
        public Guid AssetId { get; set; }
    }
}
