using System;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Boundary.Response;

namespace FinanceServicesApi.V1.UseCase.Interfaces.SuspenseTransaction
{
    public interface IGetAccountByIdUseCase
    {
        public Task<AccountResponse> ExecuteAsync(Guid id);
    }
}
