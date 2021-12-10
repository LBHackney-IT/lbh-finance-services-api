using System;

namespace FinanceServicesApi.V1.Boundary.Responses.PropertySummary
{
    public class PropertySummaryTenantsResponse
    {
        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     Kian Hayward
        /// </example>
        public string FullName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     15 Marcon Court, Amhurst Rd, Hackney, London E8 1ND
        /// </example>
        public string FullAddress { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     1501234 567 890
        /// </example>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     Kian.h@temp.com
        /// </example>
        public string Email { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     1232456789
        /// </example>
        public string TenancyId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     HRA SEC
        /// </example>
        public string TenancyType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     3
        /// </example>
        public short TimeInPropertyY { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     2
        /// </example>
        public short TimeInPropertyM { get; set; }
        public DateTime StartDate { get; set; }
    }
}
