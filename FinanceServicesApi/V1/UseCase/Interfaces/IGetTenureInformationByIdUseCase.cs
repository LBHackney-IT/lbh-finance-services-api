using System;
using System.Threading.Tasks;
using Hackney.Shared.Tenure.Domain;

namespace FinanceServicesApi.V1.UseCase.Interfaces
{
    public interface IGetTenureInformationByIdUseCase
    {
        public Task<TenureInformation> ExecuteAsync(Guid id);
    }
}
