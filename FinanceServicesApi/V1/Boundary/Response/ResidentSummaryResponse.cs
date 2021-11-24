using System;

namespace FinanceServicesApi.V1.Boundary.Response
{
    public class ResidentSummaryResponse
    {
        /// <summary>
        ///     The last balance of the all accounts of the person
        /// </summary>
        /// <example>
        ///     3215.32
        /// </example>
        public decimal CurrentBalance { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     314.25
        /// </example>
        public decimal WeeklyTotalCharges { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     23.71
        /// </example>
        public decimal ServiceCharge { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     36.14
        /// </example>
        public decimal HousingBenefit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     02/05/21
        /// </example>
        public DateTime LastPaymentDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     105.37
        /// </example>
        public decimal LastPaymentAmount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     123456789
        /// </example>
        public string TenureId { get; set; }

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
        public DateTime? TenureStartDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     123456789
        /// </example>
        public string PersonId { get; set; }

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
        ///     15 Marcon
        /// </example>
        public string PrimaryTenantAddress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     12/12/12
        /// </example>
        public DateTime? DateOfBirth { get; set; }
    }
}
