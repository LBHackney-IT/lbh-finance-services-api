using System;
using System.Threading.Tasks;
using BaseApi.V1.Boundary.Response;

namespace BaseApi.V1.UseCase.Interfaces.SuspenseTransaction.Accounts
{
    public interface IGetByIdUseCase
    {
        public Task<AccountResponse> ExecuteAsync(Guid id);
    }
}
