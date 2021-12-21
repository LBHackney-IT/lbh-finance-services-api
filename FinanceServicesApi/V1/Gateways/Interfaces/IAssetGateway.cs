using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hackney.Shared.Asset.Domain;

namespace FinanceServicesApi.V1.Gateways.Interfaces
{
    public interface IAssetGateway
    {
        public Task<Asset> GetById(Guid id);
    }
}
