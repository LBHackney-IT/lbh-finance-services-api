using FinanceServicesApi.V1.Domain.AssetModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceServicesApi.V1.Gateways.Interfaces
{
    public interface IHousingSearchGateway
    {
        Task<AssetListResponse> GetAssets(string searchText);
    }
}
