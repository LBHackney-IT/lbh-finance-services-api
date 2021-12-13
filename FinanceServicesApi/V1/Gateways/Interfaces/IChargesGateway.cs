using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Domain.Charges;

namespace FinanceServicesApi.V1.Gateways.Interfaces
{
    public interface IChargesGateway
    {
        public Task<List<Charge>> GetAllByAssetId(Guid assetId);
    }
}
