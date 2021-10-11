using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FinanceServicesApi.V1.Domain;
using FluentAssertions;
using Xunit;

namespace FinanceServicesApi.Tests.V1.Domain
{
    public class SuspenseResolutionInfoTests
    {

        private readonly Fixture _fixture;

        public SuspenseResolutionInfoTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void SuspenseResolutionInfoEntityHasPropertiesSet()
        {
            var suspenseResolutionInfoType = typeof(SuspenseResolutionInfo);
            suspenseResolutionInfoType.GetProperties().Length.Should().Be(7);

            SuspenseResolutionInfo suspenseResolutionInfo = _fixture.Create<SuspenseResolutionInfo>();

            Assert.IsType<DateTime>(suspenseResolutionInfo.ApprovedDate);
            Assert.IsType<DateTime>(suspenseResolutionInfo.ConfirmedDate);
            Assert.IsType<DateTime>(suspenseResolutionInfo.ResolutionDate);
            Assert.IsType<bool>(suspenseResolutionInfo.IsApproved);
            Assert.IsType<bool>(suspenseResolutionInfo.IsConfirmed);
            Assert.IsType<bool>(suspenseResolutionInfo.IsResolve);
            Assert.IsType<string>(suspenseResolutionInfo.Note);

        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public void IsResolveDependsIsConfirmedAndIsApproved(bool isConfirmed, bool isApproved)
        {
            SuspenseResolutionInfo suspenseResolutionInfo = _fixture.Create<SuspenseResolutionInfo>();
            suspenseResolutionInfo.IsApproved = isApproved;
            suspenseResolutionInfo.IsConfirmed = isConfirmed;

            suspenseResolutionInfo.IsResolve.Should()
                .Be(suspenseResolutionInfo.IsConfirmed && suspenseResolutionInfo.IsApproved);

        }

    }
}
