using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FinanceServicesApi.V1.Domain.AccountModels;
using FinanceServicesApi.V1.Factories;
using FinanceServicesApi.V1.Infrastructure.Entities;
using Xunit;

namespace FinanceServicesApi.Tests.V1.Factories
{
    public class AccountFactoryTests
    {

        private readonly Fixture _fixture;

        public AccountFactoryTests()
        {
            _fixture = new Fixture();
        }

        /*[Fact]
        public void ToDimainWithCorrespondingDataTypeReturnsValidData()
        {
            Guid TargetId = Guid.NewGuid();
            AccountDbEntity request = new AccountDbEntity
            {
                ConsolidatedBalance = 125,
                TargetId = TargetId,
                Tenure = 
            };
            Account response = new Account();

            AccountFactory.ToDomain();
        }*/

    }
}
