using Microsoft.AspNetCore.Mvc;

namespace FinanceServicesApi.Versioning
{
    public static class ApiVersionExtensions
    {
        public static string GetFormattedApiVersion(this ApiVersion apiVersion)
        {
            return $"v{apiVersion.ToString()}";
        }
    }
}
