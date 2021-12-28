using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime.Internal.Util;
using AutoFixture;
using AutoMapper.Internal;
using FinanceServicesApi.V1.Domain.AccountModels;
using FinanceServicesApi.V1.Domain.Charges;
using FinanceServicesApi.V1.Domain.ContactDetails;
using FinanceServicesApi.V1.Domain.TransactionModels;
using FinanceServicesApi.V1.Infrastructure.Enums;
using Hackney.Shared.Person;
using Hackney.Shared.Tenure.Domain;

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

    public static class MockToResidentSummaryResponseInput
    {
        private static readonly Fixture _fixture = new Fixture();
        public static List<object[]> GetTestData { get; private set; } = new List<object[]>
        {
            new object[]
            {
                null,null,null,null,null,null
            },
            new object[]
            {
                _fixture.Create<Person>(),null,null,null,null,null
            },
            new object[]
            {
                null,_fixture.Create<TenureInformation>(),null,null,null,null
            },
            new object[]
            {
                null,null,_fixture.Create<Account>(),null,null,null
            },
            new object[]
            {
                null,null,null,_fixture.Create<List<Charge>>(),null,null
            },
            new object[]
            {
                null,null,null,null,_fixture.Create<List<ContactDetail>>(),null
            },
            new object[]
            {
                null,null,null,null,null,_fixture.Create<List<Transaction>>()
            },
        };
    }

    /*public static class MockAccount
    {
        private static readonly Fixture _fixture = new Fixture();
        public static List<object[]> GetTestData { get; private set; } = new List<object[]>
        {
            new object[]
            {
                _fixture.Build<Account>().With(p=>p.ConsolidatedBalance,167),167
            }
        };
    }*/

    public class MockAccount : IEnumerable<object[]>
    {
        private readonly Fixture _fixture = new Fixture();

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { null, null ,null};
            yield return new object[] { _fixture.Build<Account>()
                .With(p => p.ConsolidatedBalance, 167m)
                .With(p=>p.Tenure,
                    _fixture.Build<AccountTenureSubSet>()
                        .With(v=>v.TenureId,"123456789")
                        .Create())
                .Create(), 167m ,"123456789"};
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class MockTransaction : IEnumerable<object[]>
    {
        private readonly Fixture _fixture = new Fixture();

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {
                new List<Transaction>(5){

                    {
                        _fixture.Build<Transaction>().With(t1=>t1.HousingBenefitAmount,10m).Create()
                    },
                    {
                        _fixture.Build<Transaction>().With(t1=>t1.HousingBenefitAmount,20m).Create()
                    },
                    {
                        _fixture.Build<Transaction>().With(t1=>t1.HousingBenefitAmount,30m).Create()
                    },
                    {
                        _fixture.Build<Transaction>()
                            .With(t1=>t1.HousingBenefitAmount,40m)
                            .With(t1=>t1.PaidAmount,37.5m)
                            .With(t1=>t1.TransactionDate,new DateTime(2002,12,11))
                            .Create()
                    },
                    {
                        _fixture.Build<Transaction>()
                            .With(t1=>t1.HousingBenefitAmount,50m)
                            .With(t1=>t1.PaidAmount,0)
                            .With(t1=>t1.TransactionDate,new DateTime(2002,12,12))
                            .Create()
                    }

                }, 150m, 37.5m,new DateTime(2002,12,11)};
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
