using System;
using System.ComponentModel.DataAnnotations;
using BaseApi.V1.Infrastructure;

namespace BaseApi.V1.Domain.SuspenseTransaction
{
    public class ConfirmTransferEntity
    {
        [NonEmptyGuid]
        public Guid Id { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public string Payee { get; set; }
        public static string Account => "Suspense";
        public string Address { get; set; }
        [Required]
        public decimal CurrentArrears { get; set; }
        [Required]
        public decimal ArrearsAfterPayment { get; set; }
        [Required]
        public string Resident { get; set; }
        [Required]
        public string RentAccountNumber { get; set; }
    }
}
