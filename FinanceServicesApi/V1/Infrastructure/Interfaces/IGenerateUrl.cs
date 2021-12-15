using System;

namespace FinanceServicesApi.V1.Infrastructure.Interfaces
{
    public interface IGenerateUrl<T> where T : class
    {
        public Uri Execute(Guid id);
    }
}
