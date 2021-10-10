using System;
using System.ComponentModel.DataAnnotations;
using FinanceServicesApi.V1.Infrastructure;

namespace FinanceServicesApi.V1.Domain.SuspenseTransaction
{
    public class ConfirmTransferEntity
    {
        [Required]
        public string Payee { get; set; }
        public string Account { get; } = "Suspense";
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
