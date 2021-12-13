using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceServicesApi.V1.Domain.AccountModels
{
    public class TenureType
    {
        /// <summary>
        ///     The code of the TenureType
        /// </summary>
        /// <example>
        ///     PVG
        /// </example>
        public string Code { get; set; }
        /// <summary>
        ///     The description of the tenure
        /// </summary>
        /// <example>
        ///     Private garage
        /// </example>
        public string Description { get; set; }
    }
}
