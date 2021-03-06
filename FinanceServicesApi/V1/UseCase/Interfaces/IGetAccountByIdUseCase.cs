using System;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Domain.AccountModels;

namespace FinanceServicesApi.V1.UseCase.Interfaces
{
    public interface IGetAccountByIdUseCase
    {
        public Task<Account> ExecuteAsync(Guid id);
    }
}
