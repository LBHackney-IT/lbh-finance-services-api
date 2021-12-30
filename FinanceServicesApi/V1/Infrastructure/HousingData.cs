using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace FinanceServicesApi.V1.Infrastructure
{
    public class HousingData<T> : IHousingData<T>
        where T : class
    {
        private readonly ICustomeHttpClient _client;
        private readonly IGetEnvironmentVariables<T> _getEnvironmentVariables;
        private readonly IGenerateUrl<T> _generateUrl;
        private readonly IHttpContextAccessor _contextAccessor;

        public HousingData()
        {

        }

        public HousingData(ICustomeHttpClient client, IGetEnvironmentVariables<T> getEnvironmentVariables, IGenerateUrl<T> generateUrl, IHttpContextAccessor contextAccessor)
        {
            _client = client;
            _getEnvironmentVariables = getEnvironmentVariables;
            _generateUrl = generateUrl;
            _contextAccessor = contextAccessor;
        }

        public async Task<T> DownloadAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException($"{nameof(id)} shouldn't be empty.");

            var apiToken = _contextAccessor.HttpContext.Request.Headers["Authorization"];

            _client.AddAuthorization(new AuthenticationHeaderValue("Bearer", apiToken));
            Uri uri = _generateUrl.Execute(id);

            var response = await _client.GetAsync(uri).ConfigureAwait(false);
            if (response == null)
            {
                throw new Exception($"{nameof(T)} api is not reachable!");
            }
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.StatusCode.ToString());
            }

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var tResponse = JsonConvert.DeserializeObject<T>(responseContent);
            return tResponse;
        }
    }
}
