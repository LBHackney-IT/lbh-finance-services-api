using FinanceServicesApi.V1.Boundary.Responses.PropertySummary;
using System;
using System.Threading.Tasks;

namespace FinanceServicesApi.V1.UseCase.Interfaces
{
    public interface IGetAssetAppointmentUseCase
    {
        Task<AssetAppointmentResponse> ExecuteAsync(Guid assetId, short stratPeriodYear);
    }
}