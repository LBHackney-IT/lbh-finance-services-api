using System;
using FinanceServicesApi.V1.Boundary.Responses;
using FinanceServicesApi.V1.Infrastructure.Interfaces;

namespace FinanceServicesApi.V1.Infrastructure
{
    public class GetContactEnvironmentVariables : IGetEnvironmentVariables<GetContactDetailsResponse>
    {
        public Uri GetUrl()
        {
            string result = Environment.GetEnvironmentVariable("CONTACT_DETAILS_API_URL") ?? string.Empty; ;
            if (string.IsNullOrEmpty(result))
                throw new Exception("Contact details api url shouldn't be null or empty.");
            return new Uri(result);
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
