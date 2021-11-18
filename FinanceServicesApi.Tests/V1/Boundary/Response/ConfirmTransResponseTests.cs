using AutoFixture;
using FinanceServicesApi.V1.Domain.SuspenseTransaction;
using FluentAssertions;
using Xunit;

namespace FinanceServicesApi.Tests.V1.Boundary.Response
{
    public class ConfirmTransResponseTests
    {
        private readonly Fixture _fixture = new Fixture();

        [Fact]
        public void ConfirmTransferEntityHasPropertiesSet()
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

        [Fact]
        public void AccountAlwaysReturnsSuspense()
        {
            var entity = _fixture.Create<ConfirmTransferEntity>();
            entity.Account.Should().Be("Suspense");
        }

    }
}
