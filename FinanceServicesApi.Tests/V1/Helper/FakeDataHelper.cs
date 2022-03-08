using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Amazon.DynamoDBv2.Model;
using AutoFixture;
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
                    response.Items.Add(new Dictionary<string, AttributeValue>()
                    {
                        { "id", new AttributeValue { S = _fixture.Create<Guid>().ToString() } },
                        { "target_id", new AttributeValue { S = _fixture.Create<Guid>().ToString() } },
                        { "target_type", new AttributeValue { S = _fixture.Create<TargetType>().ToString() } },
                        { "charge_group", new AttributeValue { S = _fixture.Create<ChargeGroup>().ToString() } },
                        { "charge_sub_group", new AttributeValue { S = _fixture.Create<ChargeSubGroup>().ToString() } },
                        { "charge_year", new AttributeValue { S = _fixture.Create<short>().ToString() } },
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
                    response.Items.Add(new Dictionary<string, AttributeValue>()
                    {
                        {"id", new AttributeValue {S = _fixture.Create<Guid>().ToString()}},
                        {"address", new AttributeValue {S = _fixture.Create<string>().ToString()}},
                        {"balance_amount", new AttributeValue {N = _fixture.Create<decimal>().ToString("F")}},
                        {"bank_account_number", new AttributeValue {S = _fixture.Create<string>().ToString()}},
                        {"sort_code", new AttributeValue {S = _fixture.Create<string>().ToString()}},
                        {"charged_amount", new AttributeValue {N = _fixture.Create<decimal>().ToString("F")}},
                        {"financial_month", new AttributeValue {N = _fixture.Create<int>().ToString()}},
                        {"financial_year", new AttributeValue {N = _fixture.Create<int>().ToString()}},
                        {"fund", new AttributeValue {S = _fixture.Create<string>()}},
                        {"housing_benefit_amount",new AttributeValue {N = _fixture.Create<decimal>().ToString("F")}},
                        {"is_suspense", new AttributeValue {S = _fixture.Create<bool>().ToString()}},
                        {"paid_amount", new AttributeValue {N = _fixture.Create<decimal>().ToString("F")}},
                        {"payment_reference", new AttributeValue {S = _fixture.Create<string>()}},
                        {"period_no", new AttributeValue {N = _fixture.Create<int>().ToString()}},
                        {"target_id", new AttributeValue {S = _fixture.Create<Guid>().ToString()}},
                        {"target_type", new AttributeValue {S = "Tenure"}},
                        {"last_updated_at", new AttributeValue {S = _fixture.Create<DateTime>().ToString("F")}},
                        {"last_updated_by", new AttributeValue {S = _fixture.Create<string>()}},
                        {"created_at", new AttributeValue {S = _fixture.Create<DateTime>().ToString("F")}},
                        {"created_by", new AttributeValue {S = _fixture.Create<string>()}},
                        {"transaction_amount",new AttributeValue {N = _fixture.Create<decimal>().ToString("F")}},
                        {"transaction_date", new AttributeValue {S = _fixture.Create<DateTime>().ToString("F")}},
                        {"transaction_source", new AttributeValue {S = _fixture.Create<string>()}},
                        {"transaction_type",new AttributeValue {S = _fixture.Create<TransactionType>().ToString()}},
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
    public class MockAccount : IEnumerable<object[]>
    {
        private readonly Fixture _fixture = new Fixture();

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { null, null, null };
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
                }, 150m, 37.5m,new DateTime(2002,12,11)
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    public class MockPerson : IEnumerable<object[]>
    {
        private readonly Fixture _fixture = new Fixture();

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                _fixture
                    .Build<Person>()
                    .Without(p => p.DateOfBirth)
                    .Without(p => p.FirstName)
                    .With(p => p.Surname, "Smith")
                    .Create(),
                null, "Smith"
            };
            yield return new object[]
            {
                _fixture
                    .Build<Person>()
                    .With(p => p.DateOfBirth,new DateTime(2020,02,02))
                    .Without(p => p.FirstName)
                    .With(p => p.Surname, "Smith")
                    .Create(),
                new DateTime(2020,02,02), "Smith"
            };
            yield return new object[]
            {
                _fixture
                    .Build<Person>()
                    .With(p => p.DateOfBirth,new DateTime(2020,02,02))
                    .With(p => p.FirstName,"John")
                    .Without(p => p.Surname)
                    .Create(),
                new DateTime(2020,02,02), "John"
            };
            yield return new object[] { null, null, null };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    public class MockTenure : IEnumerable<object[]>
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly Guid _tenureId = Guid.NewGuid();

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                null,null,null,null,Guid.Empty
            };
            yield return new object[]
            {
                _fixture.Build<TenureInformation>()
                    .Without(p=>p.TenuredAsset)
                    .With(p=>p.TenureType,new Hackney.Shared.Tenure.Domain.TenureType()
                    {
                        Code = "Code1",Description = "Desc1"
                    })
                    .With(p=>p.StartOfTenureDate,new DateTime(2020,01,01))
                    .With(p=>p.Id,_tenureId)
                    .Create(),
                null,"Desc1",new DateTime(2020,01,01),_tenureId
            };
            yield return new object[]
            {
                _fixture.Build<TenureInformation>()
                    .With(p=>p.TenuredAsset,
                        _fixture.Build<TenuredAsset>()
                            .Without(i=>i.FullAddress)
                            .Create()
                    )
                    .With(p=>p.TenureType,new Hackney.Shared.Tenure.Domain.TenureType()
                    {
                        Code = "Code1",Description = "Desc1"
                    })
                    .With(p=>p.StartOfTenureDate,new DateTime(2020,01,01))
                    .With(p=>p.Id,_tenureId)
                    .Create(),
                null,"Desc1",new DateTime(2020,01,01),_tenureId
            };
            yield return new object[]
            {
                _fixture.Build<TenureInformation>()
                    .With(p=>p.TenuredAsset,
                        _fixture.Build<TenuredAsset>()
                            .With(i=>i.FullAddress,"ABCDEF")
                            .Create()
                    )
                    .With(p=>p.TenureType,new Hackney.Shared.Tenure.Domain.TenureType()
                    {
                        Code = "Code1",Description = "Desc1"
                    })
                    .With(p=>p.StartOfTenureDate,new DateTime(2020,01,01))
                    .With(p=>p.Id,_tenureId)
                    .Create(),
                "ABCDEF","Desc1",new DateTime(2020,01,01),_tenureId
            };
            yield return new object[]
            {
                _fixture.Build<TenureInformation>()
                    .With(p=>p.TenuredAsset,
                        _fixture.Build<TenuredAsset>()
                            .With(i=>i.FullAddress,"ABCDEF")
                            .Create()
                    )
                    .Without(p=>p.TenureType)
                    .With(p=>p.StartOfTenureDate,new DateTime(2020,01,01))
                    .With(p=>p.Id,_tenureId)
                    .Create(),
                "ABCDEF",null,new DateTime(2020,01,01),_tenureId
            };
            yield return new object[]
            {
                _fixture.Build<TenureInformation>()
                    .With(p=>p.TenuredAsset,
                        _fixture.Build<TenuredAsset>()
                            .With(i=>i.FullAddress,"ABCDEF")
                            .Create()
                    )
                    .With(p=>p.TenureType,new Hackney.Shared.Tenure.Domain.TenureType()
                    {
                        Code = "Code1",Description = "Desc1"
                    })
                    .Without(p=>p.StartOfTenureDate)
                    .With(p=>p.Id,_tenureId)
                    .Create(),
                "ABCDEF","Desc1",null,_tenureId
            };
            yield return new object[]
            {
                _fixture.Build<TenureInformation>()
                    .With(p=>p.TenuredAsset,
                        _fixture.Build<TenuredAsset>()
                            .With(i=>i.FullAddress,"ABCDEF")
                            .Create()
                    )
                    .With(p=>p.TenureType,new Hackney.Shared.Tenure.Domain.TenureType()
                    {
                        Code = "Code1",Description = "Desc1"
                    })
                    .With(p=>p.StartOfTenureDate,new DateTime(2020,01,01))
                    .With(p=>p.Id,Guid.Empty)
                    .Create(),
                "ABCDEF","Desc1",new DateTime(2020,01,01),Guid.Empty
            };
            yield return new object[]
            {
                _fixture.Build<TenureInformation>()
                    .With(p=>p.TenuredAsset,
                        _fixture.Build<TenuredAsset>()
                            .With(i=>i.FullAddress,"ABCDEF")
                            .Create()
                    )
                    .With(p=>p.TenureType,new Hackney.Shared.Tenure.Domain.TenureType()
                    {
                        Code = "Code1",Description = "Desc1"
                    })
                    .With(p=>p.StartOfTenureDate,new DateTime(2020,01,01))
                    .Without(p=>p.Id)
                    .Create(),
                "ABCDEF","Desc1",new DateTime(2020,01,01),Guid.Empty
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    public class MockContact : IEnumerable<object[]>
    {
        private readonly Fixture _fixture = new Fixture();

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                null, null,null
            };
            yield return new object[]
            {
                new List<ContactDetail>
                {
                    {
                        _fixture.Build<ContactDetail>()
                            .With(p=>p.TargetType,TargetType.Person)
                            .With(p=>p.ContactInformation,_fixture.Build<ContactInformation>()
                                .With(p=>p.ContactType,ContactType.Email)
                                .With(p=>p.Value,"sample@site.ab")
                                .Create()
                            )
                            .Create()
                    },
                    {
                        _fixture.Build<ContactDetail>()
                            .With(p=>p.TargetType,TargetType.Person)
                            .With(p=>p.ContactInformation,_fixture.Build<ContactInformation>()
                                .With(p=>p.ContactType,ContactType.Phone)
                                .With(p=>p.Value,"+10123456879")
                                .Create()
                            )
                            .Create()
                    }
                }
                , "sample@site.ab","+10123456879"
            };
            yield return new object[]
            {
                new List<ContactDetail>
                {
                    {
                        _fixture.Build<ContactDetail>()
                            .With(p=>p.TargetType,TargetType.Person)
                            .With(p=>p.ContactInformation,_fixture.Build<ContactInformation>()
                                .With(p=>p.ContactType,ContactType.Email)
                                .With(p=>p.Value,"sample@site.ab")
                                .Create()
                            )
                            .Create()
                    }
                }
                , "sample@site.ab",null
            };
            yield return new object[]
            {
                new List<ContactDetail>
                {
                    {
                        _fixture.Build<ContactDetail>()
                            .With(p=>p.TargetType,TargetType.Person)
                            .With(p=>p.ContactInformation,_fixture.Build<ContactInformation>()
                                .With(p=>p.ContactType,ContactType.Phone)
                                .With(p=>p.Value,"+10123456879")
                                .Create()
                            )
                            .Create()
                    }
                }
                , null,"+10123456879"
            };
            yield return new object[]
            {
                new List<ContactDetail>
                {
                    {
                        _fixture.Build<ContactDetail>()
                            .With(p=>p.TargetType,TargetType.Person)
                            .With(p=>p.ContactInformation,_fixture.Build<ContactInformation>()
                                .With(p=>p.ContactType,ContactType.Email)
                                .With(p=>p.Value,"sample@site.ab")
                                .Create()
                            )
                            .Create()
                    },
                    {
                        _fixture.Build<ContactDetail>()
                            .With(p=>p.TargetType,TargetType.Person)
                            .With(p=>p.ContactInformation,_fixture.Build<ContactInformation>()
                                .With(p=>p.ContactType,ContactType.Phone)
                                .Without(p=>p.Value)
                                .Create()
                            )
                            .Create()
                    }
                }
                , "sample@site.ab",null
            };
            yield return new object[]
            {
                new List<ContactDetail>
                {
                    {
                        _fixture.Build<ContactDetail>()
                            .With(p=>p.TargetType,TargetType.Person)
                            .With(p=>p.ContactInformation,_fixture.Build<ContactInformation>()
                                .With(p=>p.ContactType,ContactType.Email)
                                .Without(p=>p.Value)
                                .Create()
                            )
                            .Create()
                    },
                    {
                        _fixture.Build<ContactDetail>()
                            .With(p=>p.TargetType,TargetType.Person)
                            .With(p=>p.ContactInformation,_fixture.Build<ContactInformation>()
                                .With(p=>p.ContactType,ContactType.Phone)
                                .With(p=>p.Value,"+10123456879")
                                .Create()
                            )
                            .Create()
                    }
                }
                , null,"+10123456879"
            };
            yield return new object[]
            {
                new List<ContactDetail>
                {
                    {
                        _fixture.Build<ContactDetail>()
                            .With(p=>p.TargetType,TargetType.Person)
                            .With(p=>p.ContactInformation,_fixture.Build<ContactInformation>()
                                .With(p=>p.ContactType,ContactType.Email)
                                .With(p=>p.Value,"sample@site.ab")
                                .Create()
                            )
                            .Create()
                    },
                    {
                        _fixture.Build<ContactDetail>()
                            .With(p=>p.TargetType,TargetType.Person)
                            .With(p=>p.ContactInformation,_fixture.Build<ContactInformation>()
                                .With(p=>p.ContactType , ContactType.Address)
                                .With(p=>p.Value,"987654")
                                .Create()
                            )
                            .Create()
                    }
                }
                , "sample@site.ab",null
            };
            yield return new object[]
            {
                new List<ContactDetail>
                {
                    {
                        _fixture.Build<ContactDetail>()
                            .With(p=>p.TargetType,TargetType.Person)
                            .With(p=>p.ContactInformation,_fixture.Build<ContactInformation>()
                                .With(p=>p.ContactType,ContactType.Address)
                                .With(p=>p.Value,"Sample@email.com")
                                .Create()
                            )
                            .Create()
                    },
                    {
                        _fixture.Build<ContactDetail>()
                            .With(p=>p.TargetType,TargetType.Person)
                            .With(p=>p.ContactInformation,_fixture.Build<ContactInformation>()
                                .With(p=>p.ContactType,ContactType.Phone)
                                .With(p=>p.Value,"+10123456879")
                                .Create()
                            )
                            .Create()
                    }
                }
                ,null,"+10123456879"
            };
            yield return new object[]
            {
                new List<ContactDetail>
                {
                    {
                        _fixture.Build<ContactDetail>()
                            .With(p=>p.TargetType,TargetType.Person)
                            .With(p=>p.ContactInformation,_fixture.Build<ContactInformation>()
                                .With(p=>p.ContactType,ContactType.Email)
                                .With(p=>p.Value,"sample@site.ab")
                                .Create()
                            )
                            .Create()
                    },
                    {
                        _fixture.Build<ContactDetail>()
                            .With(p=>p.TargetType,TargetType.Person)
                            .Without(p=>p.ContactInformation )
                            .Create()
                    }
                }
                , "sample@site.ab",null
            };
            yield return new object[]
            {
                new List<ContactDetail>
                {
                    {
                        _fixture.Build<ContactDetail>()
                            .With(p=>p.TargetType,TargetType.Person)
                            .Without(p=>p.ContactInformation)
                            .Create()
                    },
                    {
                        _fixture.Build<ContactDetail>()
                            .With(p=>p.TargetType,TargetType.Person)
                            .With(p=>p.ContactInformation,_fixture.Build<ContactInformation>()
                                .With(p=>p.ContactType,ContactType.Phone)
                                .With(p=>p.Value,"+10123456879")
                                .Create()
                            )
                            .Create()
                    }
                }
                , null,"+10123456879"
            };
            yield return new object[]
            {
                new List<ContactDetail>
                {
                    {
                        _fixture.Build<ContactDetail>()
                            .With(p=>p.TargetType,TargetType.Person)
                            .With(p=>p.ContactInformation,_fixture.Build<ContactInformation>()
                                .With(p=>p.ContactType,ContactType.Email)
                                .With(p=>p.Value,"sample@site.ab")
                                .Create()
                            )
                            .Create()
                    },
                    {
                        _fixture.Build<ContactDetail>()
                            .Without(p=>p.TargetType)
                            .With(p=>p.ContactInformation,_fixture.Build<ContactInformation>()
                                .With(p=>p.ContactType,ContactType.Phone)
                                .With(p=>p.Value,"+10123456879")
                                .Create()
                            )
                            .Create()
                    }
                }
                , "sample@site.ab",null
            };
            yield return new object[]
            {
                new List<ContactDetail>
                {
                    {
                        _fixture.Build<ContactDetail>()
                            .Without(p=>p.TargetType)
                            .With(p=>p.ContactInformation,_fixture.Build<ContactInformation>()
                                .With(p=>p.ContactType,ContactType.Email)
                                .With(p=>p.Value,"sample@site.ab")
                                .Create()
                            )
                            .Create()
                    },
                    {
                        _fixture.Build<ContactDetail>()
                            .With(p=>p.TargetType,TargetType.Person)
                            .With(p=>p.ContactInformation,_fixture.Build<ContactInformation>()
                                .With(p=>p.ContactType,ContactType.Phone)
                                .With(p=>p.Value,"+10123456879")
                                .Create()
                            )
                            .Create()
                    }
                }
                , null,"+10123456879"
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    public class MockCharges : IEnumerable<object[]>
    {
        private readonly Fixture _fixture = new Fixture();

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                null, null,null
            };
            yield return new object[]
            {
                _fixture.Build<Charge>().Without(p=>p.DetailedCharges).Create()
                , 0m,0m
            };
            yield return new object[]
            {
                _fixture.Build<Charge>()
                    .With(p=>p.DetailedCharges,new List<DetailedCharges>(4)
                    {
                        {
                            _fixture.Build<DetailedCharges>()
                                .With(p=>p.Type,"service")
                                .With(p=>p.Amount,11.35m)
                                .With(p=>p.Frequency,"Weekly")
                                .Create()
                        }
                    })
                    .Create()
                /*new List<Charge>()
                {
                    {
                        _fixture.Build<Charge>()
                            .With(p=>p.DetailedCharges,new List<DetailedCharges>(4)
                            {
                                {
                                    _fixture.Build<DetailedCharges>()
                                        .With(p=>p.Type,"service")
                                        .With(p=>p.Amount,10.25m)
                                        .With(p=>p.Frequency,"Monthly")
                                        .Create()
                                }
                            })
                            .Create()
                    },
                    {
                        _fixture.Build<Charge>()
                            .With(p=>p.DetailedCharges,new List<DetailedCharges>(4)
                            {
                                {
                                    _fixture.Build<DetailedCharges>()
                                        .With(p=>p.Type,"service")
                                        .With(p=>p.Amount,11.35m)
                                        .With(p=>p.Frequency,"Monthly")
                                        .Create()
                                }
                            })
                            .Create()
                    },
                    {
                        _fixture.Build<Charge>()
                            .With(p=>p.DetailedCharges,new List<DetailedCharges>(4)
                            {
                                {
                                    _fixture.Build<DetailedCharges>()
                                        .With(p=>p.Type,"service")
                                        .With(p=>p.Amount,11.35m)
                                        .With(p=>p.Frequency,"Weekly")
                                        .Create()
                                }
                            })
                            .Create()
                    }
                }*/
                , 32.95m,11.35m
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
