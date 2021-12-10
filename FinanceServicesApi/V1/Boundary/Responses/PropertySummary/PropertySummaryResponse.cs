using System;
using System.Collections.Generic;
using FinanceServicesApi.V1.Domain.PropertySummary;
using Hackney.Shared.Asset.Domain;

namespace FinanceServicesApi.V1.Boundary.Responses.PropertySummary
{
    public class PropertySummaryResponse
    {
        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     15 Marcon Court, Amhurst Rd, Hackney, London E8 1ND
        /// </example>
        public AssetAddress Address { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     1235.12
        /// </example>
        public decimal? CurrentBalance { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     125.54
        /// </example>
        public float? Rent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>
        /// 23.71
        /// </example>
        public decimal ServiceCharge { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     54.25
        /// </example>
        public decimal? HousingBenefit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>
        /// 56425.25
        /// </example>
        public decimal? YearToDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     5642.25     
        /// </example>
        public decimal WeeklyTotalCharges { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     5682.58
        /// </example>
        public decimal YearlyRentDebits { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     231852456
        /// </example>
        public string Prn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     HRA SEC
        /// </example>
        public string TenancyType { get; set; }
        public DateTime? TenancyStartDate { get; set; }
        public string PropertyReference { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     Kian Hayward
        /// </example>
        public string PrimaryTenantName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     01234 567 890
        /// </example>
        public string PrimaryTenantPhoneNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     Kian.h@temp.com
        /// </example>
        public string PrimaryTenantEmail { get; set; }
        public int PropertySize { get; set; }
        public PropertyDetails PropertyDetails { get; set; }
    }
}
