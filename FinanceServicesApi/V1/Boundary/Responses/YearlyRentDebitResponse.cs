namespace FinanceServicesApi.V1.Boundary.Responses
{
    public class YearlyRentDebitResponse
    {
        /// <example>
        /// 2020
        /// </example>
        public short ChargeYear { get; set; }

        /// <example>
        /// John Doe
        /// </example>
        public string LeaseHolderName { get; set; }

        /// <example>
        /// 37467586982
        /// </example>
        public string PaymentReferenceNumber { get; set; }

        /// <example>
        /// 400.00
        /// </example>
        public decimal RentCharge { get; set; }

        /// <example>
        /// 100.00
        /// </example>
        public decimal ServiceCharge { get; set; }
    }
}
