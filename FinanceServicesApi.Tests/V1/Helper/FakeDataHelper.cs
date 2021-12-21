using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;
using AutoFixture;
using FinanceServicesApi.V1.Domain.AccountModels;
using FinanceServicesApi.V1.Domain.Charges;
using FinanceServicesApi.V1.Domain.TransactionModels;
using FinanceServicesApi.V1.Infrastructure.Enums;

namespace FinanceServicesApi.Tests.V1.Helper
{
    public static class FakeDataHelper
    {
        private static readonly Fixture _fixture = new Fixture();
        public static QueryResponse MockQueryResponse<T>(int length = 1)
        {
            QueryResponse response = new QueryResponse();
            for (int i = 0; i < length; i++)
            {
                if (typeof(T) == typeof(Charge))
                {
                    response.Items.Add(
                    new Dictionary<string, AttributeValue>()
                        {
                        { "id", new AttributeValue { S = _fixture.Create<Guid>().ToString() } },
                        { "target_id", new AttributeValue { S = _fixture.Create<Guid>().ToString() } },
                        { "target_type", new AttributeValue { S = _fixture.Create<TargetType>().ToString() } },
                        { "charge_group", new AttributeValue { S = _fixture.Create<ChargeGroup>().ToString() } },
                        { "created_by", new AttributeValue { S = _fixture.Create<string>() } },
                        { "last_updated_by", new AttributeValue { S = _fixture.Create<string>() } },
                        { "created_date", new AttributeValue { S = _fixture.Create<DateTime>().ToString("F") } },
                        { "last_updated_date", new AttributeValue { S = _fixture.Create<DateTime>().ToString("F") } },
                        {
                            "detailed_charges", new AttributeValue
                            {
                                L = Enumerable.Range(0, new Random(10).Next(1, 10))
                                    .Select(p =>
                                        new AttributeValue
                                        {
                                            M =
                                            {
                                                {
                                                    "amount",
                                                    new AttributeValue
                                                    {
                                                        N = _fixture.Create<decimal>().ToString("F")
                                                    }
                                                },
                                                {
                                                    "frequency",
                                                    new AttributeValue { S = _fixture.Create<string>() }
                                                },
                                                { "type", new AttributeValue { S = _fixture.Create<string>() } },
                                                { "subType", new AttributeValue { S = _fixture.Create<string>() } },
                                                { "chargeCode", new AttributeValue { S = _fixture.Create<string>() } },
                                                { "chargeType", new AttributeValue { S = _fixture.Create<ChargeType>().ToString() } },
                                                { "startDate", new AttributeValue { S = _fixture.Create<DateTime>().ToString("F") } },
                                                { "endDate", new AttributeValue { S = _fixture.Create<DateTime>().ToString("F") } }
                                            }
                                        }
                                    ).ToList()
                            }
                        }
                        });
                }
                if (typeof(T) == typeof(Transaction))
                {
                    response.Items.Add(
                                        new Dictionary<string, AttributeValue>()
                                        {
                        {"id", new AttributeValue {S = _fixture.Create<Guid>().ToString()}},
                        {"address", new AttributeValue {S = _fixture.Create<string>().ToString()}},
                        {"balance_amount", new AttributeValue {N = _fixture.Create<decimal>().ToString("F")}},
                        {"bank_account_number", new AttributeValue {S = _fixture.Create<string>().ToString()}},
                        {"charged_amount", new AttributeValue {N = _fixture.Create<decimal>().ToString("F")}},
                        {"financial_month", new AttributeValue {N = _fixture.Create<int>().ToString()}},
                        {"financial_year", new AttributeValue {N = _fixture.Create<int>().ToString()}},
                        {"fund", new AttributeValue {S = _fixture.Create<string>()}},
                        {
                            "housing_benefit_amount",
                            new AttributeValue {N = _fixture.Create<decimal>().ToString("F")}
                        },
                        {"is_suspense", new AttributeValue {S = _fixture.Create<bool>().ToString()}},// GSI must be string or binary, so the string is implemented instead of boolean
                        {"paid_amount", new AttributeValue {N = _fixture.Create<decimal>().ToString("F")}},
                        {"payment_reference", new AttributeValue {S = _fixture.Create<string>()}},
                        {"period_no", new AttributeValue {N = _fixture.Create<int>().ToString()}},
                        {"target_id", new AttributeValue {S = _fixture.Create<Guid>().ToString()}},
                        {"last_updated_at", new AttributeValue {S = _fixture.Create<DateTime>().ToString("F")}},
                        {"last_updated_by", new AttributeValue {S = _fixture.Create<string>()}},
                        {"created_at", new AttributeValue {S = _fixture.Create<DateTime>().ToString("F")}},
                        {"created_by", new AttributeValue {S = _fixture.Create<string>()}},
                        {
                            "transaction_amount",
                            new AttributeValue {N = _fixture.Create<decimal>().ToString("F")}
                        },
                        {
                            "transaction_date",
                            new AttributeValue {S = _fixture.Create<DateTime>().ToString("F")}
                        },
                        {"transaction_source", new AttributeValue {S = _fixture.Create<string>()}},
                        {
                            "transaction_type",
                            new AttributeValue {S = _fixture.Create<TransactionType>().ToString()}
                        },
                        {
                            "person",
                            new AttributeValue
                            {
                                M = new Dictionary<string, AttributeValue>
                                {
                                    {"id", new AttributeValue {S = _fixture.Create<Guid>().ToString()}},
                                    {"fullName", new AttributeValue {S = _fixture.Create<string>()}}
                                }
                            }
                        },
                        {
                            "suspense_resolution_info",
                            new AttributeValue
                            {
                                M = new Dictionary<string, AttributeValue>
                                {
                                    {
                                        "isConfirmed",
                                        new AttributeValue {BOOL = _fixture.Create<bool>()}
                                    },
                                    {
                                        "isApproved",
                                        new AttributeValue {BOOL = _fixture.Create<bool>()}
                                    },
                                    {"note", new AttributeValue {S = _fixture.Create<string>()}},
                                    {
                                        "resolutionDate",
                                        new AttributeValue {S = _fixture.Create<DateTime>().ToString("F")}
                                    }
                                }
                            }
                        }
                    });
                }
                if (typeof(T) == typeof(Account))
                {
                    response.Items.Add(
                    new Dictionary<string, AttributeValue>()
                        {
                        { "id", new AttributeValue { S = _fixture.Create<Guid>().ToString() } },
                        { "parent_account_id", new AttributeValue { S = _fixture.Create<Guid>().ToString() } },
                        { "payment_reference", new AttributeValue { S = _fixture.Create<string>() } },
                        { "target_type", new AttributeValue { S = _fixture.Create<TargetType>().ToString() } },
                        { "target_id", new AttributeValue { S = _fixture.Create<Guid>().ToString() } },
                        { "account_type", new AttributeValue { S = _fixture.Create<AccountType>().ToString() } },
                        { "rent_group_type", new AttributeValue { S = _fixture.Create<RentGroupType>().ToString() } },
                        { "agreement_type", new AttributeValue { S = _fixture.Create<string>() } },
                        { "account_balance", new AttributeValue { N = _fixture.Create<decimal>().ToString("F") } },
                        { "consolidated_balance", new AttributeValue { N = _fixture.Create<decimal>().ToString("F") } },
                        { "created_by", new AttributeValue { S = _fixture.Create<string>() } },
                        { "last_updated_by", new AttributeValue { S = _fixture.Create<string>() } },
                        { "created_at", new AttributeValue { S = _fixture.Create<DateTime>().ToString("F") } },
                        { "last_updated_at", new AttributeValue { S = _fixture.Create<DateTime>().ToString("F") } },
                        { "start_date", new AttributeValue { S = _fixture.Create<DateTime>().ToString("F") } },
                        { "end_date", new AttributeValue { S = _fixture.Create<DateTime>().ToString("F") } },
                        { "account_status", new AttributeValue { S = _fixture.Create<AccountStatus>().ToString() } },
                        {
                            "consolidated_charges", new AttributeValue
                            {
                                L = Enumerable.Range(0, new Random(10).Next(1, 100))
                                    .Select(p =>
                                        new AttributeValue
                                        {
                                            M =
                                            {
                                                {
                                                    "amount",
                                                    new AttributeValue
                                                    {
                                                        N = _fixture.Create<decimal>().ToString("F")
                                                    }
                                                },
                                                {
                                                    "frequency",
                                                    new AttributeValue { S = _fixture.Create<string>() }
                                                },
                                                { "type", new AttributeValue { S = _fixture.Create<string>() } }
                                            }
                                        }
                                    ).ToList()
                            }
                        },
                        {
                            "tenure",
                            new AttributeValue
                            {
                                M = new Dictionary<string, AttributeValue>
                                {
                                    { "fullAddress", new AttributeValue { S = _fixture.Create<string>() } },
                                    { "tenureId", new AttributeValue { S = _fixture.Create<string>() } },
                                    { "tenureType", new AttributeValue
                                        {
                                            M = new Dictionary<string, AttributeValue>
                                            {
                                                {"code",new AttributeValue{S= _fixture.Create<string>()} },
                                                {"description",new AttributeValue{S=_fixture.Create<string>()} }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        });
                }
            }
            return response;
        }
    }

    public static class MockToConfirmTransferResponseInputs
    {
        private static readonly Fixture _fixture = new Fixture();
        public static List<object[]> GetTestData { get; private set; } = new List<object[]>
        {
            new object[]
            {
                null,null
            },
            new object[]
            {
                _fixture.Create<Account>(),null
            },
            new object[]
            {
                null,_fixture.Create<Transaction>()
            },
            new object[]
            {
                _fixture.Create<Account>(),_fixture.Create<Transaction>()
            }
        };
    }
}
