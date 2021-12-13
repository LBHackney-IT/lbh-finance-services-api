using System;
using FinanceServicesApi.V1.Infrastructure.Enums;
using FinanceServicesApi.V1.Infrastructure.Interfaces;

namespace FinanceServicesApi.V1.Infrastructure
{
    public class GetContactEnvironmentVariables : IGetEnvironmentVariables
    {
        public string GetUrl(SearchBy searchBy = default)
        {
            string result = Environment.GetEnvironmentVariable("CONTACT_DETAILS_API_URL") ?? string.Empty; ;
            if (string.IsNullOrEmpty(result))
                throw new Exception("Contact details api url shouldn't be null or empty.");
            return result;
        }

        public string GetToken()
        {
            string result = Environment.GetEnvironmentVariable("CONTACT_DETAILS_API_TOKEN") ?? string.Empty; ;
            if (string.IsNullOrEmpty(result))
                throw new Exception("Contact detail api token shouldn't be null or empty.");
            return result;
        } 

    }
}
