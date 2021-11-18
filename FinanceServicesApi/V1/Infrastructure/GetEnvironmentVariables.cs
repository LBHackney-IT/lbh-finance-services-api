using System;
using FinanceServicesApi.V1.Infrastructure.Interfaces;

namespace FinanceServicesApi.V1.Infrastructure
{
    public class GetEnvironmentVariables : IGetEnvironmentVariables
    {
        public string GetAccountApiUrl()
        {
            string result = Environment.GetEnvironmentVariable("ACCOUNT_API_URL") ?? string.Empty;
            if (string.IsNullOrEmpty(result))
                throw new Exception("Account api url shouldn't be null or empty");
            return result;
        }

        public string GetAccountApiToken()
        {
            string result = Environment.GetEnvironmentVariable("ACCOUNT_API_TOKEN") ?? string.Empty; ;
            if (string.IsNullOrEmpty(result))
                throw new Exception("Account api token shouldn't be null or empty");
            return result;
        }

        public string GetTransactionApiUrl()
        {
            string result = Environment.GetEnvironmentVariable("TRANSACTION_API_URL") ?? string.Empty;
            if (string.IsNullOrEmpty(result))
                throw new Exception("Transaction api url shouldn't be null or empty");
            return result;
        }

        public string GetTransactionApiKey()
        {
            string result = Environment.GetEnvironmentVariable("TRANSACTION_API_KEY") ?? string.Empty; ;
            if (string.IsNullOrEmpty(result))
                throw new Exception("Transaction api key shouldn't be null or empty");
            return result;
        }

        public string GetTenureInformationApiUrl()
        {
            string result = Environment.GetEnvironmentVariable("TENURE_API_URL") ?? string.Empty; ;
            if (string.IsNullOrEmpty(result))
                throw new Exception("Tenure api url shouldn't be null or empty");
            return result;
        }

        public string GetTenureInformationApiToken()
        {
            string result = Environment.GetEnvironmentVariable("TENURE_API_TOKEN") ?? string.Empty; ;
            if (string.IsNullOrEmpty(result))
                throw new Exception("Tenure api token shouldn't be null or empty");
            return result;
        }

        public string GetFinancialSummaryApiUrl()
        {
            string result = Environment.GetEnvironmentVariable("FINANCIAL_SUMMARY_API_URL") ?? string.Empty; ;
            if (string.IsNullOrEmpty(result))
                throw new Exception("Financial summary api url shouldn't be null or empty");
            return result;
        }

        public string GetFinancialSummaryApiKey()
        {
            string result = Environment.GetEnvironmentVariable("FINANCIAL_SUMMARY_API_KEY") ?? string.Empty; ;
            if (string.IsNullOrEmpty(result))
                throw new Exception("Financial summary api key shouldn't be null or empty");
            return result;
        }

        public string GetContactDetailsApiUrl()
        {
            string result = Environment.GetEnvironmentVariable("CONTACT_DETAILS_API_URL") ?? string.Empty; ;
            if (string.IsNullOrEmpty(result))
                throw new Exception("Contact details api url shouldn't be null or empty");
            return result;
        }

        public string GetContactDetailsApiToken()
        {
            string result = Environment.GetEnvironmentVariable("CONTACT_DETAILS_API_TOKEN") ?? string.Empty; ;
            if (string.IsNullOrEmpty(result))
                throw new Exception("Contact detail api token shouldn't be null or empty");
            return result;
        }

        public string GetChargesApiUrl()
        {
            string result = Environment.GetEnvironmentVariable("CHARGE_API_URL") ?? string.Empty; ;
            if (string.IsNullOrEmpty(result))
                throw new Exception("Charge api url shouldn't be null or empty");
            return result;
        }

        public string GetChargesApiKey()
        {
            string result = Environment.GetEnvironmentVariable("CHARGE_API_KEY") ?? string.Empty; ;
            if (string.IsNullOrEmpty(result))
                throw new Exception("Charge api key shouldn't be null or empty");
            return result;
        }

        public string GetAssetInformationApiUrl()
        {
            string result = Environment.GetEnvironmentVariable("ASSET_INFORMATION_API_URL") ?? string.Empty; ;
            if (string.IsNullOrEmpty(result))
                throw new Exception("Asset information api url shouldn't be null or empty");
            return result;
        }

        public string GetAssetInformationApiToken()
        {
            string result = Environment.GetEnvironmentVariable("ASSET_INFORMATION_API_TOKEN") ?? string.Empty; ;
            if (string.IsNullOrEmpty(result))
                throw new Exception("Asset information api token shouldn't be null or empty");
            return result;
        }

        public string GetPersonApiUrl()
        {
            string result = Environment.GetEnvironmentVariable("PERSON_API_URL") ?? string.Empty; ;
            if (string.IsNullOrEmpty(result))
                throw new Exception("Person api url shouldn't be null or empty");
            return result;
        }

        public string GetPersonApiToken()
        {
            string result = Environment.GetEnvironmentVariable("PERSON_API_TOKEN") ?? string.Empty; ;
            if (string.IsNullOrEmpty(result))
                throw new Exception("Person api token shouldn't be null or empty");
            return result;
        }
    }
}
