using FinanceServicesApi.V1.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Infrastructure.Enums;

namespace FinanceServicesApi.V1.Infrastructure
{
    public class HousingData<T> : IHousingData<T>
        where T : class
    {
        private readonly ICustomeHttpClient _client;
        private readonly IGenerateUrl<T> _generateUrl;
        private readonly IHttpContextAccessor _contextAccessor;

        [ExcludeFromCodeCoverage]
        public HousingData() { }

        public HousingData(ICustomeHttpClient client, IGenerateUrl<T> generateUrl, IHttpContextAccessor contextAccessor)
        {
            _client = client;
            _generateUrl = generateUrl;
            _contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Sends GET request to the API to get entity by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="searchBy"></param>
        /// <returns>Returns null if entity wasn't fount</returns>
        /// <exception cref="ArgumentException">If provided entity id is empty</exception>
        /// <exception cref="InvalidCredentialException">If request doesn't have Authorization header with JWT token</exception>
        /// <exception cref="Exception">If response is null or have unsuccessful status code</exception>
        public async Task<T> DownloadAsync(Guid id,SearchBy searchBy=SearchBy.ById)
        {
            if (id == Guid.Empty)
                throw new ArgumentException($"{nameof(id)} shouldn't be empty.");

            var apiToken = _contextAccessor.HttpContext?.Request?.Headers["Authorization"];
            if (string.IsNullOrEmpty(apiToken))
                throw new InvalidCredentialException("Api token shouldn't be null or empty.");

            _client.AddAuthorization(new AuthenticationHeaderValue("Bearer", apiToken));
            Uri uri = _generateUrl.Execute(id,searchBy);

            var response = await _client.GetAsync(uri).ConfigureAwait(false);

            if (response == null)
            {
                throw new Exception($"Housing API to get {typeof(T)} is not reachable.");
            }
            else if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                    return null;
                throw new Exception($"Exception in receiving {typeof(T)}: {response.StatusCode.ToString()}");
            }

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var tResponse = JsonConvert.DeserializeObject<T>(responseContent);
            return tResponse;
        }
    }
}
