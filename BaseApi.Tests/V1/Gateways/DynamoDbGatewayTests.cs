using System;
using Amazon.DynamoDBv2.DataModel;
using AutoFixture;
using BaseApi.Tests.V1.Helper;
using BaseApi.V1.Domain.SuspenseTransaction;
using BaseApi.V1.Gateways;
using BaseApi.V1.Infrastructure;
using FluentAssertions;
using Moq;
using Xunit;


namespace BaseApi.Tests.V1.Gateways
{
    //TODO: Remove this file if DynamoDb gateway not being used
    //TODO: Rename Tests to match gateway name
    //For instruction on how to run tests please see the wiki: https://github.com/LBHackney-IT/lbh-base-api/wiki/Running-the-test-suite.

    public class DynamoDbGatewayTests
    {
        private readonly Fixture _fixture = new Fixture();
        private Mock<IDynamoDBContext> _dynamoDb;
        private DynamoDbGateway _classUnderTest;

        public DynamoDbGatewayTests()
        {
            _dynamoDb = new Mock<IDynamoDBContext>();
            _classUnderTest = new DynamoDbGateway(_dynamoDb.Object);
        }

        [Fact]
        public void GetEntityByIdReturnsNullIfEntityDoesntExist()
        {
            var response = _classUnderTest.GetEntityById(Guid.NewGuid());

            response.Should().BeNull();
        }

        [Fact]
        public void GetEntityByIdReturnsTheEntityIfItExists()
        {
            var entity = _fixture.Create<ConfirmTransferEntity>();
            var dbEntity = DatabaseEntityHelper.CreateDatabaseEntityFrom(entity);

            _dynamoDb.Setup(x => x.LoadAsync<DatabaseEntity>(entity.Id, default))
                     .ReturnsAsync(dbEntity);

            var response = _classUnderTest.GetEntityById(entity.Id);

            _dynamoDb.Verify(x => x.LoadAsync<DatabaseEntity>(entity.Id, default), Times.Once);

            entity.Id.Should().Be(response.Id);
            entity.CreatedAt.Should().BeSameDateAs(response.CreatedAt);
        }
    }
}
