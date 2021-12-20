using System;
using Hackney.Shared.Asset.Domain;
using Hackney.Shared.Tenure.Domain;

namespace FinanceServicesApi.V1.Boundary.Responses.PropertySummary
{
    public class PropertySummaryResponse
    {
        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     15 Macron Court, Amhurst Rd, Hackney, London E8 1ND
        /// </example>
        public AssetAddress Address { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     1235.12
        /// </example>
        public float? CurrentBalance { get; set; }
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
        public decimal? ServiceCharge { get; set; }
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
        public decimal? WeeklyTotalCharges { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     5682.58
        /// </example>
        public decimal? YearlyRentDebits { get; set; }
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
        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     12/12/12
        /// </example>
        public DateTime? TenancyStartDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     25846053
        /// </example>
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
        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     3
        /// </example>
        public int PropertySize { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     Tenant
        /// </example>
        public PersonTenureType? PersonTenureType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     315000
        /// </example>
        public decimal? EstimateTotal { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     315000
        /// </example>
        public decimal? ActualTotal { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     315000
        /// </example>
        public decimal? PaidThisYear { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     315000
        /// </example>
        public decimal? ArrearsBalance { get; set; }
    }
}
