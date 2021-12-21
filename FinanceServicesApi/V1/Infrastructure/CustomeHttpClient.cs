using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Infrastructure.Interfaces;

namespace FinanceServicesApi.V1.Infrastructure
{
    public class CustomeHttpClient : HttpClient, ICustomeHttpClient
    {
        public void AddAuthorization(AuthenticationHeaderValue headerValue)
        {
            if (headerValue != null) base.DefaultRequestHeaders.Authorization = headerValue;
        }

        public void AddHeader(HttpHeader<string, string> header)
        {
            if (DefaultRequestHeaders.Any(a => a.Key.ToLower() == header.Name.ToLower()))
                DefaultRequestHeaders.Remove(header.Name);
            DefaultRequestHeaders.Add(header.Name, header.Value);
        }

        /// <inheritdoc />
        public new async Task<HttpResponseMessage> GetAsync(Uri uri)
        {
            return await base.GetAsync(uri).ConfigureAwait(false);
        }
    }
}
