using System;
using System.Threading.Tasks;
using BaseApi.V1.Boundary.Response;

namespace BaseApi.V1.Gateways.Interfaces.SuspenseTransaction
{
    public interface IAccountGateway
    {
        public Task<AccountResponse> GetById(Guid id);
    }
}
