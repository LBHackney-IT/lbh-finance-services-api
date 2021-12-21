using System;
using FinanceServicesApi.V1.Infrastructure.Interfaces;
using Hackney.Shared.Tenure.Domain;
using Microsoft.AspNetCore.Http;

namespace FinanceServicesApi.V1.Infrastructure
{
    public class GetTenureEnvironmentVariables : IGetEnvironmentVariables<TenureInformation>
    {
        private readonly IHttpContextAccessor _context;

        public GetTenureEnvironmentVariables(IHttpContextAccessor context)
        {
            _context = context;
        }
        public Uri GetUrl()
        {
            string result = Environment.GetEnvironmentVariable("TENURE_API_URL") ?? string.Empty; ;
            if (string.IsNullOrEmpty(result))
                throw new Exception("Tenure api url shouldn't be null or empty.");
            return new Uri(result);
        }

        public string GetToken()
        {
            string result = _context.HttpContext.Request.Headers["Authorization"]; //Environment.GetEnvironmentVariable("TENURE_API_TOKEN") ?? string.Empty; ;
            if (string.IsNullOrEmpty(result))
                throw new Exception("Tenure api token shouldn't be null or empty.");
            return result;
        }
    }
}
