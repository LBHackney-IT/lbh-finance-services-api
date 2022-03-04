using AutoFixture;
using FinanceServicesApi.V1.Boundary.Responses;
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
            var confirmTransferEntity = typeof(ConfirmTransferResponse);
            confirmTransferEntity.GetProperties().Length.Should().Be(8);

            var entity = _fixture.Create<ConfirmTransferResponse>();
            Assert.IsType<string>(entity.Address);
            Assert.IsType<decimal>(entity.ArrearsAfterPayment);
            Assert.IsType<decimal>(entity.CurrentArrears);
            Assert.IsType<string>(entity.Payee);
            Assert.IsType<string>(entity.RentAccountNumber);
            Assert.IsType<string>(entity.Resident);
            Assert.IsType<decimal>(entity.TotalAmount);
            Assert.IsType<string>(entity.Account);
        }

        [Fact]
        public void AccountAlwaysReturnsSuspense()
        {
            var entity = _fixture.Create<ConfirmTransferResponse>();
            entity.Account.Should().Be("Suspense");
        }

    }
}
