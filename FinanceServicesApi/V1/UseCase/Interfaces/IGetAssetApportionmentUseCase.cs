using FinanceServicesApi.V1.Boundary.Request.Enums;
using FinanceServicesApi.V1.Boundary.Responses.PropertySummary;
using System;
using System.Threading.Tasks;

namespace FinanceServicesApi.V1.UseCase.Interfaces
{
    public interface IGetAssetApportionmentUseCase
    {
        Task<AssetApportionmentResponse> ExecuteAsync(Guid assetId, short stratPeriodYear, ChargeGroupFilter chargeGroupFilter);
    }
}
