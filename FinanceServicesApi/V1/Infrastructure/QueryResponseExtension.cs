using System;
using System.Collections.Generic;
using System.Linq;
using Amazon.DynamoDBv2.Model;
using FinanceServicesApi.V1.Domain.AccountModels;
using FinanceServicesApi.V1.Domain.Charges;
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
                var chargeYear = !string.IsNullOrEmpty(item["charge_year"].N)
                        ? Convert.ToInt16(item["charge_year"].N) : 0;
                chargesList.Add(new Charge
                {
                    Id = Guid.Parse(item["id"].S),
                    TargetId = Guid.Parse(item["target_id"].S),
                    ChargeGroup = Enum.Parse<ChargeGroup>(item["charge_group"].S),
                    TargetType = Enum.Parse<TargetType>(item["target_type"].S),
                    ChargeYear = Convert.ToInt16(chargeYear),
                    DetailedCharges = detailCharges
                });
            }

            return chargesList;
        }

        public static Account ToAccount(this QueryResponse response)
        {
            foreach (Dictionary<string, AttributeValue> item in response.Items)
            {
                List<ConsolidatedCharge> consolidatedChargesList = null;

                if (item.Keys.Any(p => p == "consolidated_charges"))
                {
                    var charges = item["consolidated_charges"].L;

                    var chargesItems = charges.Select(p => p.M);
                    consolidatedChargesList = new List<ConsolidatedCharge>();
                    foreach (Dictionary<string, AttributeValue> innerItem in chargesItems)
                    {
                        consolidatedChargesList.Add(new ConsolidatedCharge
                        {
                            Amount = decimal.Parse(innerItem["amount"].N),
                            Frequency = innerItem["frequency"].S,
                            Type = innerItem["type"].S
                        });
                    }
                }

                AccountTenureSubSet tenure = null;
                if (item.Keys.Any(p => p == "tenure") && !item["tenure"].NULL)
                {
                    var tenureItem = item["tenure"].M;
                    tenure = new AccountTenureSubSet
                    {
                        FullAddress = tenureItem.ContainsKey("fullAddress") ? tenureItem["fullAddress"].S : "",
                        TenureId = tenureItem.ContainsKey("tenureId") ? tenureItem["tenureId"].S : "",
                        TenureType = new TenureType
                        {
                            Code = tenureItem.ContainsKey("tenureType") ? tenureItem["tenureType"].M.ContainsKey("code") ? tenureItem["tenureType"].M["code"].S : "" : "",
                            Description = tenureItem.ContainsKey("tenureType") ?
                                tenureItem["tenureType"].M.ContainsKey("description") ? tenureItem["tenureType"].M["description"].S : "" : ""
                        }
                    };
                    if (tenureItem.ContainsKey("primaryTenants") && !tenureItem["primaryTenants"].NULL)
                    {
                        tenure.PrimaryTenants = new List<PrimaryTenants>();
                        if (!tenureItem["primaryTenants"].NULL)
                        {
                            foreach (var primaryItems in tenureItem["primaryTenants"].L)
                            {
                                tenure.PrimaryTenants.Add(new PrimaryTenants
                                {
                                    FullName = primaryItems.M.ContainsKey("fullName") ? primaryItems.M["fullName"].S : "",
                                    Id = primaryItems.M.ContainsKey("id") ? Guid.Parse(primaryItems.M["id"].S) : Guid.Empty
                                });
                            }
                        }
                    }
                }

                return new Account
                {
                    /*Id = Guid.Parse(item["id"].S),*/
                    Id = item["id"].S,
                    AccountBalance = decimal.Parse(item["account_balance"].N),
                    ConsolidatedCharges = consolidatedChargesList,
                    Tenure = tenure,
                    TargetType = Enum.Parse<TargetType>(item["target_type"].S),
                    TargetId = Guid.Parse(item["target_id"].S),
                    AccountType = Enum.Parse<AccountType>(item["account_type"].S),
                    RentGroupType = Enum.Parse<RentGroupType>(item["rent_group_type"].S),
                    AgreementType = item.ContainsKey("agreement_type") ? item["agreement_type"].S : null,
                    CreatedBy = item.ContainsKey("created_by") ? item["created_by"].S : null,
                    LastUpdatedBy = item.ContainsKey("last_updated_by") ? item["last_updated_by"].S : null,
                    CreatedAt = DateTime.Parse(item["created_at"].S),
                    LastUpdatedAt = DateTime.Parse(item["last_updated_at"].S),
                    StartDate = DateTime.Parse(item["start_date"].S),
                    EndDate = item.ContainsKey("end_date") ? DateTime.Parse(item["end_date"].S) : (DateTime?) null,
                    AccountStatus = Enum.Parse<AccountStatus>(item["account_status"].S),
                    PaymentReference = item["payment_reference"].S,
                    ParentAccountId = Guid.Parse(item["parent_account_id"].S)
                };
            }

            return null;
        }
    }
}
