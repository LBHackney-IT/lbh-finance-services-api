using System;
using System.Threading.Tasks;
using Hackney.Shared.Asset.Domain;

namespace FinanceServicesApi.V1.UseCase.Interfaces
{
    public interface IGetAssetByIdUseCase
    {
        public Task<Asset> ExecuteAsync(Guid id);
    }
}
