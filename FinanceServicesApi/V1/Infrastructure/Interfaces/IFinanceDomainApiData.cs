using System;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Infrastructure.Enums;

namespace FinanceServicesApi.V1.Infrastructure.Interfaces
{
    public interface IFinanceDomainApiData<T> where T : class
    {
        public abstract Task<T> DownloadAsync(Guid id, SearchBy searchBy = SearchBy.ById);
    }
}
