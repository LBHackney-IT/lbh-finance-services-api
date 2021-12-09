using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Domain.Charges;
using FinanceServicesApi.V1.Infrastructure.Enums;

namespace FinanceServicesApi.V1.UseCase.Interfaces
{
    public interface IGetChargeByAssetIdUseCase
    {
        public Task<List<Charge>> ExecuteAsync(Guid assetId);
    }
}
