using FinanceServicesApi.V1.Domain.AccountModels;
using FinanceServicesApi.V1.Infrastructure.Entities;

namespace FinanceServicesApi.V1.Factories
{
    public static class AccountFactory
    {
        public static Account ToDomain(this AccountDbEntity databaseEntity)
        {
            return new Account
            {
                Id = databaseEntity.Id,
                ParentAccountId = databaseEntity.ParentAccountId,
                PaymentReference = databaseEntity.PaymentReference,
                EndReasonCode = databaseEntity.EndReasonCode,
                AccountBalance = databaseEntity.AccountBalance,
                ConsolidatedBalance = databaseEntity.ConsolidatedBalance,
                AccountStatus = databaseEntity.AccountStatus,
                EndDate = databaseEntity.EndDate,
                CreatedBy = databaseEntity.CreatedBy,
                CreatedAt = databaseEntity.CreatedAt,
                LastUpdatedBy = databaseEntity.LastUpdatedBy,
                LastUpdatedAt = databaseEntity.LastUpdatedAt,
                StartDate = databaseEntity.StartDate,
                TargetId = databaseEntity.TargetId,
                TargetType = databaseEntity.TargetType,
                AccountType = databaseEntity.AccountType,
                AgreementType = databaseEntity.AgreementType,
                RentGroupType = databaseEntity.RentGroupType,
                ConsolidatedCharges = databaseEntity.ConsolidatedCharges,
                Tenure = databaseEntity.Tenure
            };
        }
    }
}
