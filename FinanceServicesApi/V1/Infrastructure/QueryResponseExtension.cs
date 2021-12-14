using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Amazon.DynamoDBv2.Model;
using FinanceServicesApi.V1.Domain.Charges;
using FinanceServicesApi.V1.Domain.FinancialSummary;
using FinanceServicesApi.V1.Domain.TransactionModels;
using FinanceServicesApi.V1.Infrastructure.Enums;

namespace FinanceServicesApi.V1.Infrastructure
{
    public static class QueryResponseExtension
    {
        public static List<Transaction> ToTransactions(this QueryResponse response)
        {
            List<Transaction> transactions = new List<Transaction>();
            foreach (Dictionary<string, AttributeValue> item in response.Items)
            {
                SuspenseResolutionInfo suspenseResolutionInfo = null;
                TransactionPerson person = null;

                if (item.Keys.Any(p => p == "suspense_resolution_info"))
                {
                    var innerItem = item["suspense_resolution_info"].M;
                    suspenseResolutionInfo = new SuspenseResolutionInfo
                    {
                        IsConfirmed = innerItem["isConfirmed"].BOOL,
                        IsApproved = innerItem["isApproved"].BOOL,
                        Note = innerItem["note"].S,
                        ResolutionDate = DateTime.Parse(innerItem["resolutionDate"].S)
                    };
                }

                if (item.Keys.Any(p => p == "person"))
                {
                    var innerItem = item["person"].M;
                    person = new TransactionPerson
                    {
                        Id = Guid.Parse(innerItem["id"].S),
                        FullName = innerItem["fullName"].S
                    };
                }

                transactions.Add(new Transaction
                {

                    Id = Guid.Parse(item["id"].S),
                    Address = item["address"].S,
                    BalanceAmount = decimal.Parse(item["balance_amount"].N),
                    BankAccountNumber = item["bank_account_number"].S,
                    ChargedAmount = decimal.Parse(item["charged_amount"].N),
                    FinancialMonth = short.Parse(item["financial_month"].N),
                    FinancialYear = short.Parse(item["financial_year"].N),
                    Fund = item["fund"].S,
                    HousingBenefitAmount = decimal.Parse(item["housing_benefit_amount"].N),
                    PaidAmount = decimal.Parse(item["paid_amount"].N),
                    PaymentReference = item["payment_reference"].S,
                    PeriodNo = short.Parse(item["period_no"].N),
                    Person = person,
                    SuspenseResolutionInfo = suspenseResolutionInfo,
                    TargetId = Guid.Parse(item["target_id"].S),
                    TransactionAmount = decimal.Parse(item["transaction_amount"].N),
                    TransactionDate = DateTime.Parse(item["transaction_date"].S),
                    TransactionSource = item["transaction_source"].S,
                    TransactionType = Enum.Parse<TransactionType>(item["transaction_type"].S),
                    LastUpdatedBy = item.ContainsKey("last_updated_by") ? item["last_updated_by"].S : null,
                    LastUpdatedAt = DateTime.Parse(item["last_updated_at"].S),
                    CreatedBy = item.ContainsKey("created_by") ? item["created_by"].S : null,
                    CreatedAt = DateTime.Parse(item["created_at"].S),
                });
            }

            return transactions;
        }

        /*public static List<WeeklySummary> ToWeeklySummary(this QueryResponse response)
        {
            if (response is null)
            {
                throw new ArgumentNullException(nameof(response));
            }

            List<WeeklySummary> weeklySummaries = new List<WeeklySummary>();

            foreach (Dictionary<string, AttributeValue> item in response.Items)
            {
                weeklySummaries.Add(new WeeklySummary
                {
                    Id = Guid.Parse(item["id"].S),
                    TargetId = Guid.Parse(item["target_id"].S),
                    PeriodNo = short.Parse(item["period_no"].N),
                    FinancialYear = short.Parse(item["financial_year"].N),
                    FinancialMonth = short.Parse(item["financial_month"].N),
                    WeekStartDate = DateTime.Parse(item["week_start_date"].S),
                    ChargedAmount = decimal.Parse(item["charged_amount"].N, CultureInfo.InvariantCulture),
                    PaidAmount = decimal.Parse(item["paid_amount"].N, CultureInfo.InvariantCulture),
                    BalanceAmount = decimal.Parse(item["balance_amount"].N, CultureInfo.InvariantCulture),
                    HousingBenefitAmount = decimal.Parse(item["housing_benefit_amount"].N, CultureInfo.InvariantCulture),
                    SubmitDate = DateTime.Parse(item["submit_date"].S),
                });
            }

            return weeklySummaries;
        }*/

        public static List<Charge> ToCharge(this QueryResponse response)
        {
            var chargesList = new List<Charge>();
            foreach (Dictionary<string, AttributeValue> item in response.Items)
            {
                var detailCharges = new List<DetailedCharges>();
                var innerItem = item["detailed_charges"].L;
                foreach (var detail in innerItem)
                {
                    detailCharges.Add(new DetailedCharges
                    {
                        Amount = Convert.ToDecimal(detail.M["amount"].N),
                        ChargeCode = detail.M["chargeCode"].S,
                        ChargeType = Enum.Parse<ChargeType>(detail.M["chargeType"].S),
                        Type = detail.M["type"].S,
                        SubType = detail.M["subType"].S,
                        Frequency = detail.M["frequency"].S,
                        StartDate = DateTime.Parse(detail.M["startDate"].S),
                        EndDate = DateTime.Parse(detail.M["endDate"].S)
                    });
                }

                chargesList.Add(new Charge
                {
                    Id = Guid.Parse(item["id"].S),
                    TargetId = Guid.Parse(item["target_id"].S),
                    ChargeGroup = Enum.Parse<ChargeGroup>(item["charge_group"].S),
                    TargetType = Enum.Parse<TargetType>(item["target_type"].S),
                    DetailedCharges = detailCharges
                });
            }

            return chargesList;
        }
    }
}
