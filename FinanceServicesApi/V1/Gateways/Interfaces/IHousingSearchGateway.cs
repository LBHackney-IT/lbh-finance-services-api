using FinanceServicesApi.V1.Boundary.Request.Enums;
using FinanceServicesApi.V1.Domain.AssetModels;
using System.Threading.Tasks;

namespace FinanceServicesApi.V1.Gateways.Interfaces
{
    public interface IHousingSearchGateway
    {
        Task<AssetListResponse> GetAssets(string searchText, AssetType assetType);
    }
}
