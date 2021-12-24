using System;
using FinanceServicesApi.V1.Infrastructure.Enums;

namespace FinanceServicesApi.V1.Infrastructure.Interfaces
{
    public interface IGetEnvironmentVariables<T> where T : class
    {
        public Uri GetUrl();
    }
}
