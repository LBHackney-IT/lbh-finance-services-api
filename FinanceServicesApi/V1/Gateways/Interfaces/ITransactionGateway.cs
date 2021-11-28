using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Domain;

namespace FinanceServicesApi.V1.Gateways.Interfaces
{
    public interface ITransactionGateway
    {
        public Task<Transaction> GetById(Guid id);
        public Task<List<Transaction>> GetByTargetId(Guid targetId);
    }
}
