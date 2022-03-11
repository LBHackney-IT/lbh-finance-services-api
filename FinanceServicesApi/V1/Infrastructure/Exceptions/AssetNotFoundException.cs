using System;

namespace FinanceServicesApi.V1.Infrastructure.Exceptions
{
    /// <summary>
    /// Exception that generates message in format $"Asset with id [{assetId}] in source '{entitySource}' cannot be found!". 
    /// Will be handled in <see cref="FinanceServicesApi.V1.ExceptionMiddleware"/> as 404 Http Status Code
    /// </summary>
    public class AssetNotFoundException : Exception
    {
        public AssetNotFoundException(Guid assetId, string entitySource)
            : base($"Asset with id [{assetId}] in source '{entitySource}' cannot be found!")
        {
        }
    }
}
