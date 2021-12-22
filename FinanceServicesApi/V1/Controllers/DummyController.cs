using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Domain.TransactionModels;
using FinanceServicesApi.V1.Infrastructure.Enums;
using FinanceServicesApi.V1.UseCase.Interfaces;
using Hackney.Shared.Tenure.Domain;

namespace FinanceServicesApi.V1.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/v1/dummy/transaction")]
    [ApiVersion("1.0")]
    public class DummyController : BaseController
    {
        private readonly IGetTenureInformationByIdUseCase _tenure;
        private readonly Random _generator = new Random();
        public DummyController(IGetTenureInformationByIdUseCase tenure)
        {
            _tenure = tenure;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransaction(Guid id)
        {
            var tenure = await _tenure.ExecuteAsync(id).ConfigureAwait(false);
            var houseHolder = tenure.HouseholdMembers.First(p => p.PersonTenureType == PersonTenureType.Leaseholder);
            Transaction transaction = new Transaction
            {
                TransactionAmount = (decimal) _generator.Next(0, 1000000),
                Address = tenure.TenuredAsset.FullAddress,
                BalanceAmount = (decimal) _generator.Next(0, 1000000),
                BankAccountNumber = $"******{_generator.Next(11,99)}",
                ChargedAmount = (decimal) _generator.Next(0, 1000000),
                CreatedAt = RandomDay(),
                CreatedBy = Faker.Lorem.GetFirstWord(),
                FinancialMonth = (short)_generator.Next(1,12),
                FinancialYear = (short) _generator.Next(2015, 2022),
                Fund = Faker.Lorem.GetFirstWord(),
                HousingBenefitAmount = (decimal) _generator.Next(0, 1000000),
                Id = Guid.NewGuid(),
                LastUpdatedAt = RandomDay(),
                LastUpdatedBy = Faker.Lorem.GetFirstWord(),
                PaidAmount = (decimal) _generator.Next(0, 1000000),
                PaymentReference = tenure.PaymentReference,
                PeriodNo = (short) _generator.Next(1,10),
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
                TransactionType = (TransactionType) ((short)_generator.Next(1, (int)Enum.GetValues(typeof(TransactionType)).Cast<TransactionType>().Max()))
            };
            return Ok(transaction);
        }

        DateTime RandomDay()
        {
            DateTime start = new DateTime(2015, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(_generator.Next(range));
        }
    }
}
