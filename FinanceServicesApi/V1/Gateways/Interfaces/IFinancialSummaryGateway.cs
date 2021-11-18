using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Domain.FinancialSummary;

namespace FinanceServicesApi.V1.Gateways.Interfaces
{
    public interface IFinancialSummaryGateway
    {
        public Task<List<WeeklySummary>> GetGetAllByTargetId(Guid targetId, DateTime? startDate, DateTime? endDate);
    }
}
