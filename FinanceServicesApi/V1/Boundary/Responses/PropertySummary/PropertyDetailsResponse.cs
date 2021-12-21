using System.Collections.Generic;
using FinanceServicesApi.V1.Domain.Charges;

namespace FinanceServicesApi.V1.Boundary.Responses.PropertySummary
{
    public class PropertyDetailsResponse
    {
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
        ///     500000
        /// </example>
        public decimal? PropertyValue { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     100000
        /// </example>
        public decimal? The1999Value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     3
        /// </example>
        public int? Bedrooms { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     Formula
        /// </example>
        public string RentModel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     130
        /// </example>
        public decimal? WeeklyCharge { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     6760
        /// </example>
        public decimal? YearlyCharge { get; set; }

        public List<ExtraCharge> ExtraCharges { get; set; }
    }
}
