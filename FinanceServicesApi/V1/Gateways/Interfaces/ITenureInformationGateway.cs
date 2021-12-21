using System;
using System.Threading.Tasks;
using Hackney.Shared.Tenure.Domain;

namespace FinanceServicesApi.V1.Gateways.Interfaces
{
    public interface ITenureInformationGateway
    {
        public Task<TenureInformation> GetById(Guid id);
    }
}
