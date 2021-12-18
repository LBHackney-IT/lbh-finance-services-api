using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Domain.AccountModels;
using FinanceServicesApi.V1.Gateways;
using FinanceServicesApi.V1.UseCase.Interfaces;

namespace FinanceServicesApi.V1.UseCase
{
    public class GetAccountByTargetIdUseCase: IGetAccountByTargetIdUseCase
    {
        private readonly AccountGateway _gateway;

        public GetAccountByTargetIdUseCase(AccountGateway gateway)
        {
            _gateway = gateway;
        }
        public Task<Account> ExecuteAsync(Guid targetId)
        {
            if(targetId==Guid.Empty)
                throw new NullReferenceException()
            _gateway.GetByTargetId(targetId);
        }
    }
}
