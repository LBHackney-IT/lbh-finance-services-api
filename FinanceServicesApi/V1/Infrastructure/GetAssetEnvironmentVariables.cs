using System;
using FinanceServicesApi.V1.Infrastructure.Interfaces;
using Hackney.Shared.Asset.Domain;
using Microsoft.AspNetCore.Http;

namespace FinanceServicesApi.V1.Infrastructure
{
    public class GetAssetEnvironmentVariables : IGetEnvironmentVariables<Asset>
    {
        private readonly IHttpContextAccessor _context;

        public GetAssetEnvironmentVariables(IHttpContextAccessor context)
        {
            _context = context;
        }
        public Uri GetUrl()
        {
            string result = Environment.GetEnvironmentVariable("ASSET_INFORMATION_API_URL") ?? string.Empty; ;
            if (string.IsNullOrEmpty(result))
                throw new Exception("Asset information api url shouldn't be null or empty.");
            return new Uri(result);
        }

        public string GetToken()
        {
            string result = _context.HttpContext.Request.Headers["Authorization"]; //Environment.GetEnvironmentVariable("ASSET_INFORMATION_API_TOKEN") ?? string.Empty; ;
            if (string.IsNullOrEmpty(result))
                throw new Exception("Asset information api token shouldn't be null or empty.");
            return result;
        }
    }
}
