using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Domain.FinancialSummary;
using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.UseCase.Interfaces;

namespace FinanceServicesApi.V1.UseCase
{
    public class GetFinancialSummaryByTargetIdUseCase : IGetFinancialSummaryByTargetIdUseCase
    {
        private readonly IFinancialSummaryByTargetIdGateway _byTargetIdGateway;

        public GetFinancialSummaryByTargetIdUseCase(IFinancialSummaryByTargetIdGateway byTargetIdGateway)
        {
            _byTargetIdGateway = byTargetIdGateway;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetId">Asset id</param>
        /// <param name="startDate">Start date of the search</param>
        /// <param name="endDate">End date of the search</param>
        /// <returns></returns>
        public async Task<List<WeeklySummary>> ExecuteAsync(Guid targetId, DateTime? startDate, DateTime? endDate)
        {
            return await _byTargetIdGateway.GetGetAllByTargetId(targetId, startDate, endDate).ConfigureAwait(false);
        }
    }
}
