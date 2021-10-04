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
            var entity = _fixture.Create<ConfirmTransferEntity>();
            entity.Id.Should().NotBeEmpty();
        }

        [Fact]
        public void EntitiesHaveACreatedAt()
        {
            var entity = new ConfirmTransferEntity();
            var date = new DateTime(2019, 02, 21);
            entity.CreatedAt = date;

            entity.CreatedAt.Should().BeSameDateAs(date);
        }
    }
}
