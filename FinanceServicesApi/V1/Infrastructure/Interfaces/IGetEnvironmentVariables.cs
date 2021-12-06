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
        public string GetContactDetailsApiUrl();
        public string GetContactDetailsApiToken();
        public string GetChargesApiUrl();
        public string GetChargesApiKey();
        public string GetAssetInformationApiUrl();
        public string GetAssetInformationApiToken();
        public string GetPersonApiUrl();
        public string GetPersonApiToken();
        public string GetSearchApiUrl();
        public string GetSearchApiAuthKey();
    }
}
