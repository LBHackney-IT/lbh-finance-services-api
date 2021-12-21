using System;
using System.Net.Http;
using FinanceServicesApi.V1.Infrastructure.Interfaces;
using Hackney.Shared.Person;
using Microsoft.AspNetCore.Http;

namespace FinanceServicesApi.V1.Infrastructure
{
    public class GetPersonEnvironmentVariables : IGetEnvironmentVariables<Person>
    {
        private readonly IHttpContextAccessor _context;

        public GetPersonEnvironmentVariables(IHttpContextAccessor context)
        {
            _context = context;
        }
        public Uri GetUrl()
        {
            string result = Environment.GetEnvironmentVariable("PERSON_API_URL") ?? string.Empty;
            if (string.IsNullOrEmpty(result))
                throw new Exception("Person api url shouldn't be null or empty.");
            return new Uri(result);
        }

        public string GetToken()
        {
            string result = _context.HttpContext.Request.Headers["Authorization"]; //Environment.GetEnvironmentVariable("PERSON_API_TOKEN") ?? string.Empty;
            if (string.IsNullOrEmpty(result))
                throw new Exception("Person api token shouldn't be null or empty.");
            return result;
        }
    }
}
