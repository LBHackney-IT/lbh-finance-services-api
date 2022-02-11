using FinanceServicesApi.V1.Boundary.Responses.PropertySummary;
using System;
using System.Threading.Tasks;

namespace FinanceServicesApi.V1.UseCase.Interfaces
{
    public interface IGetChargesSummaryByTypeUseCase
    {
        Task<AssetAppointmentResponse> ExecuteAsync(Guid assetId, AssetType assetType);
    }
}
