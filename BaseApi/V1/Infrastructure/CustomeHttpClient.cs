using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BaseApi.V1.Infrastructure.Interfaces;

namespace BaseApi.V1.Infrastructure
{
    public class CustomeHttpClient : HttpClient, ICustomeHttpClient
    {
        public void AddAuthorization(AuthenticationHeaderValue headerValue)
        {
            if (headerValue != null) base.DefaultRequestHeaders.Authorization = headerValue;
        }

        public void AddHeader(HttpHeader<string, string> header)
        {
            DefaultRequestHeaders.Add(header.Name, header.Value);
        }

        /// <inheritdoc />
        public new async Task<HttpResponseMessage> GetAsync(Uri uri)
        {
            return await base.GetAsync(uri).ConfigureAwait(false);
        }
    }
}
