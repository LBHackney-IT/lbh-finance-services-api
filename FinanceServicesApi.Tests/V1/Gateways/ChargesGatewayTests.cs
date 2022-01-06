using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using AutoFixture;
using FinanceServicesApi.Tests.V1.Helper;
using FinanceServicesApi.V1.Domain.Charges;
using FinanceServicesApi.V1.Gateways;
using FinanceServicesApi.V1.Infrastructure;
using FluentAssertions;
using Moq;
using Xunit;

namespace FinanceServicesApi.Tests.V1.Gateways
{
    public class ChargesGatewayTests
    {
        private readonly Mock<IAmazonDynamoDB> _amazonDynamoDb = new Mock<IAmazonDynamoDB>();
        private readonly Fixture _fixture;
        private ChargesGateway _sut;

        public ChargesGatewayTests()
        {
            _fixture = new Fixture();
            _sut = new ChargesGateway(_amazonDynamoDb.Object);
        }

        [Fact]
        public void GetAllByAssetIdWithEmptyInputReturnsException()
        {
            Func<Task<List<Charge>>> func = async () => await _sut.GetAllByAssetId(Guid.Empty).ConfigureAwait(false);
            func.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void GetAllByAssetIdWitValidInputReturnsData()
        {
            QueryResponse response = FakeDataHelper.MockQueryResponse<Charge>(1);

            _amazonDynamoDb.Setup(_ => _.QueryAsync(It.IsAny<QueryRequest>(), CancellationToken.None))
                .ReturnsAsync(response);

            Func<Task<List<Charge>>> func = async () => await _sut.GetAllByAssetId(Guid.NewGuid()).ConfigureAwait(false);
            var result = func.Invoke();
            result.Should().NotBeNull();
            result.Result.Should().BeEquivalentTo(response.ToCharge());
        }

        [Fact]
        public void GetAllByAssetIdWitNonExistsIdReturnsEmptyList()
        {
            QueryResponse response = FakeDataHelper.MockQueryResponse<Charge>(0);

            _amazonDynamoDb.Setup(_ => _.QueryAsync(It.IsAny<QueryRequest>(), CancellationToken.None))
                .ReturnsAsync(response);

            Func<Task<List<Charge>>> func = async () => await _sut.GetAllByAssetId(Guid.NewGuid()).ConfigureAwait(false);
            var result = func.Invoke();
            result.Should().NotBeNull();
            result.Result.Count.Should().Be(0);
        }
    }
}
