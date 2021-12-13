using FinanceServicesApi.V1.Domain.AccountModels;
using FinanceServicesApi.V1.Infrastructure.Entities;

namespace FinanceServicesApi.V1.Factories
{
    public static class AccountFactory
    {
        public static Account ToDomain(this AccountDbEntity databaseEntity)
        {
            Account account = new Account
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
            return account;
        }

        public static AccountDbEntity ToDatabase(this Account account)
        {
            return new AccountDbEntity
            {
                Id = account.Id,
                AccountBalance = account.AccountBalance,
                ConsolidatedBalance = account.ConsolidatedBalance,
                AccountStatus = account.AccountStatus,
                EndDate = account.EndDate,
                CreatedBy = account.CreatedBy,
                CreatedAt = account.CreatedAt,
                LastUpdatedBy = account.LastUpdatedBy,
                LastUpdatedAt = account.LastUpdatedAt,
                StartDate = account.StartDate,
                TargetId = account.TargetId,
                TargetType = account.TargetType,
                AccountType = account.AccountType,
                AgreementType = account.AgreementType,
                RentGroupType = account.RentGroupType,
                ConsolidatedCharges = account.ConsolidatedCharges,
                Tenure = account.Tenure,
                PaymentReference = account.PaymentReference,
                EndReasonCode = account.EndReasonCode,
                ParentAccountId = account.ParentAccountId
            };
        }
    }
}
