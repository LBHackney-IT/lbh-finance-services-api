using System;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Domain.Charges;

namespace FinanceServicesApi.V1.UseCase.Interfaces
{
    public interface IGetChargeByTargetIdUseCase
    {
        public Task<Charge> ExecuteAsync(Guid targetId,TargetType targetType);
    }
}
