using System;
using System.Net.Http;
using System.Net.Http.Headers;
using FinanceServicesApi.V1.Infrastructure.Interfaces;

namespace FinanceServicesApi.V1.Infrastructure
{
    public class CustomeHttpClient : HttpClient, ICustomeHttpClient
    {
        public CustomeHttpClient()
        {
            base.Timeout = TimeSpan.FromMinutes(3);
        }
        public void AddAuthorization(AuthenticationHeaderValue headerValue)
        {
            if (headerValue != null) base.DefaultRequestHeaders.Authorization = headerValue;
        }
    }
}
