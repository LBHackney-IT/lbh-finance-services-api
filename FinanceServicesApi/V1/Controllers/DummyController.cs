using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Domain.AccountModels;
using FinanceServicesApi.V1.Domain.Charges;
using FinanceServicesApi.V1.Domain.TransactionModels;
using FinanceServicesApi.V1.Infrastructure.Enums;
using FinanceServicesApi.V1.UseCase.Interfaces;
using Hackney.Shared.Tenure.Domain;

namespace FinanceServicesApi.V1.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/v1/dummy")]
    [ApiVersion("1.0")]
    public class DummyController : BaseController
    {
        private readonly IGetTenureInformationByIdUseCase _tenureById;
        private readonly IGetChargeByAssetIdUseCase _chargeByAssetId;
        private readonly IGetLastPaymentTransactionsByTargetIdUseCase _transactionByTargetId;
        private readonly IGetPersonByIdUseCase _personByIdUseCase;
        private readonly Random _generator = new Random();
        public DummyController(IGetTenureInformationByIdUseCase tenureById,
            IGetChargeByAssetIdUseCase chargeByAssetId,
            IGetLastPaymentTransactionsByTargetIdUseCase transactionByTargetId,
            IGetPersonByIdUseCase personByIdUseCase)
        {
            _tenureById = tenureById;
            _chargeByAssetId = chargeByAssetId;
            _transactionByTargetId = transactionByTargetId;
            _personByIdUseCase = personByIdUseCase;
        }

        [HttpPost("tenures")]
        public async Task<IActionResult> GetTenures(List<Guid> ids)
        {
            List<TenureInformation> tenures = new List<TenureInformation>();
            foreach (Guid id in ids)
            {
                var tenure = await _tenureById.ExecuteAsync(id).ConfigureAwait(false);
                var housHold = tenure.HouseholdMembers.FirstOrDefault(m =>
                    m.PersonTenureType == PersonTenureType.Leaseholder
                    || m.PersonTenureType == PersonTenureType.Tenant);
                if (housHold == null)
                    continue;
                var person = await _personByIdUseCase.ExecuteAsync(housHold.Id).ConfigureAwait(false);
                if (person == null)
                    continue;

                var asset = await _chargeByAssetId.ExecuteAsync(tenure.TenuredAsset.Id).ConfigureAwait(false);
                if (asset == null)
                    continue;

                tenures.Add(tenure);
            }

            return Ok(tenures.Select(p => p.Id));
        }

        [HttpPost("transactions")]
        public async Task<IActionResult> GetTransaction(List<Guid> ids)
        {
            List<Transaction> response = new List<Transaction>();
            foreach (Guid id in ids)
            {

                var tenure = await _tenureById.ExecuteAsync(id).ConfigureAwait(false);
                var houseHolder = tenure.HouseholdMembers.FirstOrDefault(p => p.PersonTenureType == PersonTenureType.Leaseholder ||
                                                                              p.PersonTenureType == PersonTenureType.Tenant);
                Transaction transaction = new Transaction
                {
                    TransactionAmount = (decimal) _generator.Next(0, 1000000),
                    Address = tenure.TenuredAsset.FullAddress,
                    BalanceAmount = (decimal) _generator.Next(0, 1000000),
                    BankAccountNumber = $"******{_generator.Next(11, 99)}",
                    ChargedAmount = (decimal) _generator.Next(0, 1000000),
                    CreatedAt = RandomDay(),
                    CreatedBy = Faker.Lorem.GetFirstWord(),
                    FinancialMonth = (short) _generator.Next(1, 12),
                    FinancialYear = (short) _generator.Next(2015, 2022),
                    Fund = Faker.Lorem.GetFirstWord(),
                    HousingBenefitAmount = (decimal) _generator.Next(0, 1000000),
                    Id = Guid.NewGuid(),
                    LastUpdatedAt = RandomDay(),
                    LastUpdatedBy = Faker.Lorem.GetFirstWord(),
                    PaidAmount = (decimal) _generator.Next(0, 1000000),
                    PaymentReference = tenure.PaymentReference,
                    PeriodNo = (short) _generator.Next(1, 10),
                    Person = houseHolder == null ? null : new TransactionPerson
                    {
                        FullName = houseHolder.FullName,
                        Id = houseHolder.Id
                    },
                    SortCode = $"{_generator.Next(10, 99)}-{_generator.Next(10, 99)}-{_generator.Next(10, 99)}",
                    SuspenseResolutionInfo = null,
                    TargetId = tenure.Id,
                    TargetType = TargetType.Tenure,
                    TransactionDate = RandomDay(),
                    TransactionSource = Faker.Lorem.Words(5).First(),
                    TransactionType = (TransactionType) ((short) _generator.Next(1, (int) Enum.GetValues(typeof(TransactionType)).Cast<TransactionType>().Max()))
                };
                response.Add(transaction);
            }
            return Ok(response);
        }

        [HttpPost("accounts")]
        public async Task<IActionResult> GetAccounts(List<Guid> ids)
        {
            List<Account> response = new List<Account>();
            foreach (var id in ids)
            {
                var tenure = await _tenureById.ExecuteAsync(id).ConfigureAwait(false);
                var charge = await _chargeByAssetId.ExecuteAsync(tenure.TenuredAsset.Id).ConfigureAwait(false);
                var transaction = await _transactionByTargetId.ExecuteAsync(tenure.Id).ConfigureAwait(false);
                Account account = new Account
                {
                    PaymentReference = tenure.PaymentReference,
                    AccountBalance = charge.Sum(c => c.DetailedCharges.Sum(d => d.Amount)) -
                                     transaction.Sum(p => p.PaidAmount),
                    TargetType = TargetType.Tenure,
                    TargetId = tenure.Id,
                    CreatedBy = "Dummy",
                    LastUpdatedBy = "Dummy",
                    CreatedAt = RandomDay(),
                    LastUpdatedAt = RandomDay(),
                    AccountStatus = AccountStatus.Active,
                    AccountType = (AccountType) ((short) _generator.Next(1, (int) Enum.GetValues(typeof(AccountType)).Cast<AccountType>().Max())),
                    AgreementType = Faker.Lorem.GetFirstWord(),
                    ConsolidatedBalance = charge.Sum(c => c.DetailedCharges.Sum(d => d.Amount)) -
                                          transaction.Sum(p => p.PaidAmount),
                    ConsolidatedCharges = new List<ConsolidatedCharge>
                    {
                        new ConsolidatedCharge
                        {
                            Type = "Service",Frequency = "Monthly",Amount = charge.Sum(c=>
                                c.DetailedCharges.Where(cd=>
                                    cd.Type.ToLower()=="service" &&
                                    cd.Frequency.ToLower()=="monthly").Sum(s=>s.Amount))
                        },
                        new ConsolidatedCharge
                        {
                            Type = "Service",Frequency = "Weekly",Amount = charge.Sum(c=>
                                c.DetailedCharges.Where(cd=>
                                    cd.Type.ToLower()=="service" &&
                                    cd.Frequency.ToLower()=="weekly").Sum(s=>s.Amount))
                        },
                        new ConsolidatedCharge
                        {
                            Type = "Rent",Frequency = "Monthly",Amount = charge.Sum(c=>
                                c.DetailedCharges.Where(cd=>
                                    cd.Type.ToLower()=="rent" &&
                                    cd.Frequency.ToLower()=="monthly").Sum(s=>s.Amount))
                        },
                        new ConsolidatedCharge
                        {
                            Type = "Rent",Frequency = "Weekly",Amount = charge.Sum(c=>
                                c.DetailedCharges.Where(cd=>
                                    cd.Type.ToLower()=="rent" &&
                                    cd.Frequency.ToLower()=="weekly").Sum(s=>s.Amount))
                        }
                    },
                    EndDate = null,
                    EndReasonCode = null,
                    Id = Guid.NewGuid(),
                    ParentAccountId = Guid.Empty,
                    RentGroupType = (RentGroupType) ((short) _generator.Next(1, (int) Enum.GetValues(typeof(RentGroupType)).Cast<RentGroupType>().Max())),
                    StartDate = RandomDay(),
                    Tenure = new AccountTenureSubSet
                    {
                        FullAddress = tenure.TenuredAsset.FullAddress,
                        TenureId = Faker.RandomNumber.Next(1000, 99999).ToString(),
                        TenureType = new Domain.AccountModels.TenureType
                        {
                            Code = Faker.Lorem.GetFirstWord(),
                            Description = Faker.Lorem.GetFirstWord()
                        },
                        PrimaryTenants = tenure.HouseholdMembers.Select(p => new PrimaryTenants
                        {
                            FullName = p.FullName,
                            Id = p.Id
                        }).ToList()
                    }
                };
                response.Add(account);
            }

            return Ok(response);
        }

        [HttpPost("charges")]
        public async Task<IActionResult> GetCharges(List<Guid> ids)
        {
            List<Charge> charges = new List<Charge>();
            foreach (Guid id in ids)
            {
                var tenure = await _tenureById.ExecuteAsync(id).ConfigureAwait(false);
                if (tenure == null)
                    continue;

                if (tenure.HouseholdMembers.Any(p => p.PersonTenureType == PersonTenureType.Leaseholder))
                {
                    Charge charge = new Charge
                    {
                        Id = Guid.NewGuid(),
                        ChargeGroup = ChargeGroup.Leaseholders,
                        TargetId = tenure.TenuredAsset.Id,
                        TargetType = TargetType.Asset,
                        DetailedCharges = new List<DetailedCharges>
                        {
                            new DetailedCharges
                            {
                                Type = "Service",
                                SubType = "Block Cleaning",
                                ChargeType = ChargeType.Block,
                                ChargeCode = "DCB",
                                Frequency = "weekly",
                                Amount = (decimal) _generator.Next(0, 100),
                                StartDate = RandomDay(),
                                EndDate = RandomDay()
                            },
                            new DetailedCharges
                            {
                                Type = "Service",
                                SubType = "Estate Cleaning",
                                ChargeType = ChargeType.Estate,
                                ChargeCode = "DCE",
                                Frequency = "weekly",
                                Amount = (decimal) _generator.Next(0, 100),
                                StartDate = RandomDay(),
                                EndDate = RandomDay()
                            },
                            new DetailedCharges
                            {
                                Type = "Service",
                                SubType = "Grounds Maintenance",
                                ChargeType = ChargeType.Estate,
                                ChargeCode = "DGM",
                                Frequency = "weekly",
                                Amount = (decimal) _generator.Next(0, 100),
                                StartDate = RandomDay(),
                                EndDate = RandomDay()
                            },
                            new DetailedCharges
                            {
                                Type = "Service",
                                SubType = "Lighting",
                                ChargeType = ChargeType.Estate,
                                ChargeCode = "DDE",
                                Frequency = "weekly",
                                Amount = (decimal) _generator.Next(0, 100),
                                StartDate = RandomDay(),
                                EndDate = RandomDay()
                            },
                            new DetailedCharges
                            {
                                Type = "Service",
                                SubType = "CCTV",
                                ChargeType = ChargeType.Estate,
                                ChargeCode = "DCC",
                                Frequency = "weekly",
                                Amount = (decimal) _generator.Next(0, 100),
                                StartDate = RandomDay(),
                                EndDate = RandomDay()
                            },
                            new DetailedCharges
                            {
                                Type = "Service",
                                SubType = "Concierge Service",
                                ChargeType = ChargeType.Estate,
                                ChargeCode = "DCO",
                                Frequency = "weekly",
                                Amount = (decimal) _generator.Next(0, 100),
                                StartDate = RandomDay(),
                                EndDate = RandomDay()
                            },
                            new DetailedCharges
                            {
                                Type = "Service",
                                SubType = "Heating Fuel",
                                ChargeType = ChargeType.Block,
                                ChargeCode = "DHE",
                                Frequency = "weekly",
                                Amount = (decimal) _generator.Next(0, 100),
                                StartDate = RandomDay(),
                                EndDate = RandomDay()
                            },
                            new DetailedCharges
                            {
                                Type = "Service",
                                SubType = "Communal TV Aerial",
                                ChargeType = ChargeType.Block,
                                ChargeCode = "DCT",
                                Frequency = "weekly",
                                Amount = (decimal) _generator.Next(0, 100),
                                StartDate = RandomDay(),
                                EndDate = RandomDay()
                            },
                            new DetailedCharges
                            {
                                Type = "Service",
                                SubType = "Tenants Levy",
                                ChargeType = ChargeType.Block,
                                ChargeCode = "DTL",
                                Frequency = "weekly",
                                Amount = (decimal) _generator.Next(0, 100),
                                StartDate = RandomDay(),
                                EndDate = RandomDay()
                            },
                            new DetailedCharges
                            {
                                Type = "Service",
                                SubType = "Insurance",
                                ChargeType = ChargeType.Property,
                                ChargeCode = "DCI",
                                Frequency = "weekly",
                                Amount = (decimal) _generator.Next(0, 100),
                                StartDate = RandomDay(),
                                EndDate = RandomDay()
                            },
                            new DetailedCharges
                            {
                                Type = "Rent",
                                SubType = "Rent",
                                ChargeType = ChargeType.Property,
                                ChargeCode = "DBR",
                                Frequency = "weekly",
                                Amount = (decimal) _generator.Next(0, 100),
                                StartDate = RandomDay(),
                                EndDate = RandomDay()
                            },
                            new DetailedCharges
                            {
                                Type = "Valuation",
                                SubType = "Current",
                                ChargeType = ChargeType.Property,
                                ChargeCode = "DBR",
                                Frequency = "NA",
                                Amount = (decimal) _generator.Next(0, 100),
                                StartDate = RandomDay(),
                                EndDate = RandomDay()
                            },
                            new DetailedCharges
                            {
                                Type = "Valuation",
                                SubType = "1999",
                                ChargeType = ChargeType.Property,
                                ChargeCode = "DBR",
                                Frequency = "NA",
                                Amount = (decimal) _generator.Next(0, 100),
                                StartDate = RandomDay(),
                                EndDate = RandomDay()
                            },
                        }
                    };
                    charges.Add(charge);
                }
                else
                {
                    Charge charge = new Charge
                    {
                        Id = Guid.NewGuid(),
                        ChargeGroup = ChargeGroup.Tenants,
                        TargetId = tenure.TenuredAsset.Id,
                        TargetType = TargetType.Asset,
                        DetailedCharges = new List<DetailedCharges>
                        {
                            new DetailedCharges
                            {
                                Type = "Service",
                                SubType = "Block Cleaning",
                                ChargeType = ChargeType.Block,
                                ChargeCode = "DCB",
                                Frequency = "weekly",
                                Amount = (decimal) _generator.Next(0, 100),
                                StartDate = RandomDay(),
                                EndDate = RandomDay()
                            },
                            new DetailedCharges
                            {
                                Type = "Service",
                                SubType = "Estate Cleaning",
                                ChargeType = ChargeType.Estate,
                                ChargeCode = "DCE",
                                Frequency = "weekly",
                                Amount = (decimal) _generator.Next(0, 100),
                                StartDate = RandomDay(),
                                EndDate = RandomDay()
                            },
                            new DetailedCharges
                            {
                                Type = "Service",
                                SubType = "Grounds Maintenance",
                                ChargeType = ChargeType.Estate,
                                ChargeCode = "DGM",
                                Frequency = "weekly",
                                Amount = (decimal) _generator.Next(0, 100),
                                StartDate = RandomDay(),
                                EndDate = RandomDay()
                            },
                            new DetailedCharges
                            {
                                Type = "Service",
                                SubType = "Lighting",
                                ChargeType = ChargeType.Estate,
                                ChargeCode = "DDE",
                                Frequency = "weekly",
                                Amount = (decimal) _generator.Next(0, 100),
                                StartDate = RandomDay(),
                                EndDate = RandomDay()
                            },
                            new DetailedCharges
                            {
                                Type = "Service",
                                SubType = "CCTV",
                                ChargeType = ChargeType.Estate,
                                ChargeCode = "DCC",
                                Frequency = "weekly",
                                Amount = (decimal) _generator.Next(0, 100),
                                StartDate = RandomDay(),
                                EndDate = RandomDay()
                            },
                            new DetailedCharges
                            {
                                Type = "Service",
                                SubType = "Concierge Service",
                                ChargeType = ChargeType.Estate,
                                ChargeCode = "DCO",
                                Frequency = "weekly",
                                Amount = (decimal) _generator.Next(0, 100),
                                StartDate = RandomDay(),
                                EndDate = RandomDay()
                            },
                            new DetailedCharges
                            {
                                Type = "Service",
                                SubType = "Heating Fuel",
                                ChargeType = ChargeType.Block,
                                ChargeCode = "DHE",
                                Frequency = "weekly",
                                Amount = (decimal) _generator.Next(0, 100),
                                StartDate = RandomDay(),
                                EndDate = RandomDay()
                            },
                            new DetailedCharges
                            {
                                Type = "Service",
                                SubType = "Communal TV Aerial",
                                ChargeType = ChargeType.Block,
                                ChargeCode = "DCT",
                                Frequency = "weekly",
                                Amount = (decimal) _generator.Next(0, 100),
                                StartDate = RandomDay(),
                                EndDate = RandomDay()
                            },
                            new DetailedCharges
                            {
                                Type = "Service",
                                SubType = "Tenants Levy",
                                ChargeType = ChargeType.Block,
                                ChargeCode = "DTL",
                                Frequency = "weekly",
                                Amount = (decimal) _generator.Next(0, 100),
                                StartDate = RandomDay(),
                                EndDate = RandomDay()
                            },
                            new DetailedCharges
                            {
                                Type = "Service",
                                SubType = "Insurance",
                                ChargeType = ChargeType.Property,
                                ChargeCode = "DCI",
                                Frequency = "weekly",
                                Amount = (decimal) _generator.Next(0, 100),
                                StartDate = RandomDay(),
                                EndDate = RandomDay()
                            },
                            new DetailedCharges
                            {
                                Type = "Rent",
                                SubType = "Rent",
                                ChargeType = ChargeType.Property,
                                ChargeCode = "DBR",
                                Frequency = "weekly",
                                Amount = (decimal) _generator.Next(0, 100),
                                StartDate = RandomDay(),
                                EndDate = RandomDay()
                            },
                            new DetailedCharges
                            {
                                Type = "Valuation",
                                SubType = "Current",
                                ChargeType = ChargeType.Property,
                                ChargeCode = "DBR",
                                Frequency = "NA",
                                Amount = (decimal) _generator.Next(0, 100),
                                StartDate = RandomDay(),
                                EndDate = RandomDay()
                            },
                            new DetailedCharges
                            {
                                Type = "Valuation",
                                SubType = "1999",
                                ChargeType = ChargeType.Property,
                                ChargeCode = "DBR",
                                Frequency = "NA",
                                Amount = (decimal) _generator.Next(0, 100),
                                StartDate = RandomDay(),
                                EndDate = RandomDay()
                            },
                        }
                    };
                    charges.Add(charge);
                }
            }
            return Ok(charges);
        }

        DateTime RandomDay()
        {
            DateTime start = new DateTime(2015, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(_generator.Next(range));
        }
    }
}
