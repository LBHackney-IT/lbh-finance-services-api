using System.ComponentModel.DataAnnotations;

namespace FinanceServicesApi.V1.Boundary.Responses
{
    public class ConfirmTransferResponse
    {
        [Required]
        public string Payee { get; set; }
        public string Account { get; } = "Suspense";
        public string Address { get; set; }
        [Required]
        public decimal? CurrentArrears { get; set; }
        [Required]
        public decimal ArrearsAfterPayment { get; set; }
        [Required]
        public string Resident { get; set; }
        [Required]
        public string RentAccountNumber { get; set; }
        [Required]
        public decimal TotalAmount { get; set; }
    }
}
