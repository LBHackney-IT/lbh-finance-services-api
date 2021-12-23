namespace FinanceServicesApi.V1.Domain.AccountModels
{
    public class AccountBalanceUpdateModel
    {
        public string Value { get; set; }
        public string Path { get; set; }
        public string Op { get; set; }
    }
}
