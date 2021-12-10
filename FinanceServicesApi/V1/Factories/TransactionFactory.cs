using FinanceServicesApi.V1.Infrastructure.Entities;
using System.Collections.Generic;
using System.Linq;
using FinanceServicesApi.V1.Domain.TransactionModels;

namespace FinanceServicesApi.V1.Factories
{
    public static class TransactionFactory
    {
        public static TransactionDbEntity ToDatabase(this Transaction transaction)
        {
            return transaction == null ? null : new TransactionDbEntity
            {
                Id = transaction.Id,
                TargetId = transaction.TargetId,
                TargetType = transaction.TargetType,
                BalanceAmount = transaction.BalanceAmount,
                ChargedAmount = transaction.ChargedAmount,
                FinancialMonth = transaction.FinancialMonth,
                FinancialYear = transaction.FinancialYear,
                HousingBenefitAmount = transaction.HousingBenefitAmount,
                PaidAmount = transaction.PaidAmount,
                PaymentReference = transaction.PaymentReference,
                BankAccountNumber = transaction.BankAccountNumber,
                SuspenseResolutionInfo = transaction.SuspenseResolutionInfo,
                PeriodNo = transaction.PeriodNo,
                TransactionAmount = transaction.TransactionAmount,
                TransactionDate = transaction.TransactionDate,
                TransactionType = transaction.TransactionType,
                TransactionSource = transaction.TransactionSource,
                Address = transaction.Address,
                Person = transaction.Person,
                Fund = transaction.Fund,
                SortCode = transaction.SortCode,
                CreatedAt = transaction.CreatedAt,
                CreatedBy = transaction.CreatedBy,
                LastUpdatedBy = transaction.LastUpdatedBy,
                LastUpdatedAt = transaction.LastUpdatedAt
            };
        }

        public static Transaction ToDomain(this TransactionDbEntity transactionDbEntity)
        {
            return transactionDbEntity == null ? null : new Transaction
            {
                Id = transactionDbEntity.Id,
                TargetId = transactionDbEntity.TargetId,
                TargetType = transactionDbEntity.TargetType,
                BalanceAmount = transactionDbEntity.BalanceAmount,
                ChargedAmount = transactionDbEntity.ChargedAmount,
                FinancialMonth = transactionDbEntity.FinancialMonth,
                FinancialYear = transactionDbEntity.FinancialYear,
                HousingBenefitAmount = transactionDbEntity.HousingBenefitAmount,
                PaidAmount = transactionDbEntity.PaidAmount,
                PaymentReference = transactionDbEntity.PaymentReference,
                BankAccountNumber = transactionDbEntity.BankAccountNumber,
                SuspenseResolutionInfo = transactionDbEntity.SuspenseResolutionInfo,
                PeriodNo = transactionDbEntity.PeriodNo,
                TransactionAmount = transactionDbEntity.TransactionAmount,
                TransactionDate = transactionDbEntity.TransactionDate,
                TransactionType = transactionDbEntity.TransactionType,
                TransactionSource = transactionDbEntity.TransactionSource,
                Address = transactionDbEntity.Address,
                Person = transactionDbEntity.Person,
                Fund = transactionDbEntity.Fund,
                SortCode = transactionDbEntity.SortCode,
                CreatedAt = transactionDbEntity.CreatedAt,
                CreatedBy = transactionDbEntity.CreatedBy,
                LastUpdatedAt = transactionDbEntity.LastUpdatedAt,
                LastUpdatedBy = transactionDbEntity.LastUpdatedBy
            };
        }

        public static List<Transaction> ToDomain(this IEnumerable<TransactionDbEntity> databaseEntity)
        {
            return databaseEntity.Select(p => p.ToDomain())
                                 .OrderBy(x => x.TransactionDate)
                                 .ToList();
        }

        public static List<TransactionDbEntity> ToDatabaseList(this List<Transaction> transactions)
        {
            return transactions.Select(item => item.ToDatabase()).ToList();
        }

    }
}
