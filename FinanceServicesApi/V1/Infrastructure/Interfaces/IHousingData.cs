using System;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Infrastructure.Enums;

namespace FinanceServicesApi.V1.Infrastructure.Interfaces
{
    public interface IHousingData<T> where T : class
    {
        public Task<T> DownloadAsync(Guid id);
    }
}
