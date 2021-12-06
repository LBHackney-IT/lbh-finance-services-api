using System;
using System.Threading.Tasks;
using Hackney.Shared.HousingSearch.Domain.Accounts;

namespace FinanceServicesApi.V1.UseCase.Interfaces
{
    public interface IGetAccountByIdUseCase
    {
        public Task<Account> ExecuteAsync(Guid id);
    }
}
