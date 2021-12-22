using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Domain.AccountModels;
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
        private readonly Random _generator = new Random();
        public DummyController(IGetTenureInformationByIdUseCase tenureById, IGetChargeByAssetIdUseCase chargeByAssetId, IGetLastPaymentTransactionsByTargetIdUseCase transactionByTargetId)
        {
            _tenureById = tenureById;
            _chargeByAssetId = chargeByAssetId;
            _transactionByTargetId = transactionByTargetId;
        }

        [HttpGet("transactions/{id}")]
        public async Task<IActionResult> GetTransaction(Guid id)
        {
            var tenure = await _tenureById.ExecuteAsync(id).ConfigureAwait(false);
            var houseHolder = tenure.HouseholdMembers.First(p => p.PersonTenureType == PersonTenureType.Leaseholder);
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
                Person = new TransactionPerson
                {
                    FullName = houseHolder.FullName,
                    Id = houseHolder.Id
                },
                SortCode = "20-45-78",
                SuspenseResolutionInfo = null,
                TargetId = tenure.Id,
                TargetType = TargetType.Tenure,
                TransactionDate = RandomDay(),
                TransactionSource = Faker.Lorem.Words(5).First(),
                TransactionType = (TransactionType) ((short) _generator.Next(1, (int) Enum.GetValues(typeof(TransactionType)).Cast<TransactionType>().Max()))
            };
            return Ok(transaction);
        }

        [HttpGet("accounts")]
        public async Task<IActionResult> GetAccounts([FromQuery]List<Guid> ids)
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
                        TenureId = Faker.RandomNumber.Next(1000,99999).ToString(),
                        TenureType = new Domain.AccountModels.TenureType
                        {
                            Code = Faker.Lorem.GetFirstWord(),
                            Description = Faker.Lorem.GetFirstWord()
                        },
                        PrimaryTenants = tenure.HouseholdMembers.Select(p=> new PrimaryTenants
                        {
                            FullName = p.FullName,
                            Id = p.Id
                        } ).ToList()
                    }
                };
                response.Add(account);
            }

            return Ok(response);
        }

        DateTime RandomDay()
        {
            DateTime start = new DateTime(2015, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(_generator.Next(range));
        }
    }
}
