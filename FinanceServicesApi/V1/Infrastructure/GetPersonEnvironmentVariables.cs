using System;
using FinanceServicesApi.V1.Infrastructure.Interfaces;
using Hackney.Shared.Person;

namespace FinanceServicesApi.V1.Infrastructure
{
    public class GetPersonEnvironmentVariables : IGetEnvironmentVariables<Person>
    {
        public Uri GetUrl()
        {
            string result = Environment.GetEnvironmentVariable("PERSON_API_URL") ?? string.Empty;
            if (string.IsNullOrEmpty(result))
                throw new Exception("Person api url shouldn't be null or empty.");
            return new Uri(result);
        }

        public string GetToken()
        {
            string result = Environment.GetEnvironmentVariable("PERSON_API_TOKEN") ?? string.Empty;
            if (string.IsNullOrEmpty(result))
                throw new Exception("Person api token shouldn't be null or empty.");
            return result;
        }
    }
}
