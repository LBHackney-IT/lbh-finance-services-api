using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FinanceServicesApi.V1.Infrastructure.Interfaces
{
    public interface ICustomeHttpClient
    {
        public void AddAuthorization(AuthenticationHeaderValue headerValue);
        public Task<HttpResponseMessage> GetAsync(Uri uri);
    }
}
