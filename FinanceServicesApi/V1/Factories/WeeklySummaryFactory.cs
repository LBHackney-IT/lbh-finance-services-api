using System.Collections.Generic;
using System.Linq;
using FinanceServicesApi.V1.Domain.FinancialSummary;
using FinanceServicesApi.V1.Infrastructure.Entities;
using FinanceServicesApi.V1.Infrastructure.Enums;

namespace FinanceServicesApi.V1.Factories
{
    public static class WeeklySummaryFactory
    {
        public static WeeklySummary ToDomain(this WeeklySummaryDbEntity databaseEntity)
        {
            return databaseEntity == null ? null : new WeeklySummary
            {
                Id = databaseEntity.Id,
                TargetId = databaseEntity.TargetId,
                BalanceAmount = databaseEntity.BalanceAmount,
                ChargedAmount = databaseEntity.ChargedAmount,
                FinancialMonth = databaseEntity.FinancialMonth,
                FinancialYear = databaseEntity.FinancialYear,
                HousingBenefitAmount = databaseEntity.HousingBenefitAmount,
                PaidAmount = databaseEntity.PaidAmount,
                PeriodNo = databaseEntity.PeriodNo,
                WeekStartDate = databaseEntity.WeekStartDate,
                SubmitDate = databaseEntity.SubmitDate
            };
        }


        public static List<WeeklySummary> ToDomain(this IEnumerable<WeeklySummaryDbEntity> databaseEntity)
        {
            return databaseEntity.Select(p => p.ToDomain())
                                .OrderByDescending(x => x.WeekStartDate)
                                .ToList();
        }

        public static WeeklySummaryDbEntity ToDatabase(this WeeklySummary entity)
        {
            return entity == null ? null : new WeeklySummaryDbEntity
            {
                Id = entity.Id,
                TargetId = entity.TargetId,
                BalanceAmount = entity.BalanceAmount,
                ChargedAmount = entity.ChargedAmount,
                FinancialMonth = entity.FinancialMonth,
                FinancialYear = entity.FinancialYear,
                HousingBenefitAmount = entity.HousingBenefitAmount,
                PaidAmount = entity.PaidAmount,
                PeriodNo = entity.PeriodNo,
                WeekStartDate = entity.WeekStartDate,
                SummaryType = SummaryType.WeeklySummary,
                SubmitDate = entity.SubmitDate
            };
        }

    }
}
