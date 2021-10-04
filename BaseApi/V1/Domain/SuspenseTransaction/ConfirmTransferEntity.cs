using System;
using System.ComponentModel.DataAnnotations;
using BaseApi.V1.Infrastructure;

namespace BaseApi.V1.Domain.SuspenseTransaction
{
    public class ConfirmTransferEntity
    {
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
