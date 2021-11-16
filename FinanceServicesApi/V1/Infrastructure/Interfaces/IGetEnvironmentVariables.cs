namespace FinanceServicesApi.V1.Infrastructure.Interfaces
{
    public interface IGetEnvironmentVariables
    {
        public string GetAccountApiUrl();
        public string GetAccountApiToken();
        public string GetTransactionApiUrl();
        public string GetTransactionApiKey();
        public string GetTenureInformationApiUrl();
        public string GetTenureInformationApiToken();
        public string GetFinancialSummaryApiUrl();
        public string GetFinancialSummaryApiKey();

    }
}
