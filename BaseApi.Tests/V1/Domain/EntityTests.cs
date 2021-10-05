using System;
using AutoFixture;
using BaseApi.V1.Domain.SuspenseTransaction;
using FluentAssertions;
using Xunit;

namespace BaseApi.Tests.V1.Domain
{
    public class EntityTests
    {
        private readonly Fixture _fixture = new Fixture();

        [Fact]
        public void EntitiesHaveAnId()
        {
            var confirmTransferEntity = typeof(ConfirmTransferEntity);
            confirmTransferEntity.GetProperties().Length.Should().Be(7);

            var entity = _fixture.Create<ConfirmTransferEntity>();
            Assert.IsType<string>(entity.Address);
            Assert.IsType<decimal>(entity.ArrearsAfterPayment);
            Assert.IsType<decimal>(entity.CurrentArrears);
            Assert.IsType<string>(entity.Payee);
            Assert.IsType<string>(entity.RentAccountNumber);
            Assert.IsType<string>(entity.Resident);
        }
    }
}
