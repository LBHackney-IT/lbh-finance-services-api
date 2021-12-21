using System;
using System.ComponentModel.DataAnnotations;
using FinanceServicesApi.V1.Infrastructure;

namespace FinanceServicesApi.V1.Domain.AccountModels
{
    public class PrimaryTenants
    {
        /// <example>
        ///     793dd4ca-d7c4-4110-a8ff-c58eac4b90fa
        /// </example>
        [NonEmptyGuid]
        public Guid Id { get; set; }
        /// <example>
        ///     Smith Johnson
        /// </example>
        [Required]
        public string FullName { get; set; }
    }
}
