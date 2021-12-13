using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Domain.FinancialSummary;

namespace FinanceServicesApi.V1.UseCase.Interfaces
{
    public interface IGetFinancialSummaryByTargetIdUseCase
    {
        public Task<List<WeeklySummary>> ExecuteAsync(Guid targetId, DateTime? startDate, DateTime? endDate);
    }
}
