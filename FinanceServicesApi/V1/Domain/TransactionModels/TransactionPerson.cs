using System;
using System.ComponentModel.DataAnnotations;
using FinanceServicesApi.V1.Infrastructure;

namespace FinanceServicesApi.V1.Domain.TransactionModels
{
    public class TransactionPerson
    {
        [NonEmptyGuid("PersonId")]
        public Guid Id { get; set; }

        [Required]
        public string FullName { get; set; }
    }
}
