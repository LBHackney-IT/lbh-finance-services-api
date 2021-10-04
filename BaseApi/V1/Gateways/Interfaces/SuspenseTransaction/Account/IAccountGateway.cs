using System;
using System.Threading.Tasks;
using BaseApi.V1.Boundary.Response;

namespace BaseApi.V1.Gateways.Interfaces.SuspenseTransaction.Account
{
    public interface IAccountGateway
    {
        Task<AccountResponse> GetEntityById(Guid id);
    }
}
