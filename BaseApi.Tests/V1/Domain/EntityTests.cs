using System;
using BaseApi.V1.Domain;
using BaseApi.V1.Domain.SuspenseTransaction;
using FluentAssertions;
using NUnit.Framework;

namespace BaseApi.Tests.V1.Domain
{
    [TestFixture]
    public class EntityTests
    {
        [Test]
        public void EntitiesHaveAnId()
        {
            var entity = new ConfirmTransferEntity();
            entity.Id.Should().NotBeEmpty();
        }

        [Test]
        public void EntitiesHaveACreatedAt()
        {
            var entity = new ConfirmTransferEntity();
            var date = new DateTime(2019, 02, 21);
            entity.CreatedAt = date;

            entity.CreatedAt.Should().BeSameDateAs(date);
        }
    }
}
