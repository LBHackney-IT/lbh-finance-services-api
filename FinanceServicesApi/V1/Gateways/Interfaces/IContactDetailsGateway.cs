using System;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Boundary.Responses;

namespace FinanceServicesApi.V1.Gateways.Interfaces
{
    public interface IContactDetailsGateway
    {
        public Task<GetContactDetailsResponse> GetByTargetId(Guid targetId);

    }
}
