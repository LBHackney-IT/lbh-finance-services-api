using Amazon.DynamoDBv2.DataModel;
using System;
using FinanceServicesApi.V1.Infrastructure.Enums;
using Hackney.Core.DynamoDb.Converters;

namespace FinanceServicesApi.V1.Infrastructure.Entities
{
    [DynamoDBTable("FinancialSummaries", LowerCamelCaseProperties = true)]
    public class WeeklySummaryDbEntity
    {
        [DynamoDBHashKey(AttributeName = "target_id")]
        public Guid TargetId { get; set; }
        [DynamoDBRangeKey(AttributeName = "id")]
        public Guid Id { get; set; }

        [DynamoDBProperty(AttributeName = "summary_type", Converter = typeof(DynamoDbEnumConverter<SummaryType>))]
        public SummaryType SummaryType { get; set; }

        [DynamoDBProperty(AttributeName = "submit_date", Converter = typeof(DynamoDbDateTimeConverter))]
        public DateTime SubmitDate { get; set; }

        [DynamoDBProperty(AttributeName = "period_no")]
        public short PeriodNo { get; set; }

        [DynamoDBProperty(AttributeName = "financial_year")]
        public short FinancialYear { get; set; }

        [DynamoDBProperty(AttributeName = "financial_month")]
        public short FinancialMonth { get; set; }

        [DynamoDBProperty(AttributeName = "week_start_date", Converter = (typeof(DynamoDbDateTimeConverter)))]
        public DateTime WeekStartDate { get; set; }

        [DynamoDBProperty(AttributeName = "paid_amount")]
        public decimal PaidAmount { get; set; }

        [DynamoDBProperty(AttributeName = "charged_amount")]
        public decimal ChargedAmount { get; set; }

        [DynamoDBProperty(AttributeName = "balance_amount")]
        public decimal BalanceAmount { get; set; }

        [DynamoDBProperty(AttributeName = "housing_benefit_amount")]
        public decimal HousingBenefitAmount { get; set; }
    }
}
