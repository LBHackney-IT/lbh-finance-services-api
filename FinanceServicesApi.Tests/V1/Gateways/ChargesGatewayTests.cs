using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using FinanceServicesApi.V1.Domain.Charges;
using FinanceServicesApi.V1.Gateways;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace FinanceServicesApi.Tests.V1.Gateways
{
    public class ChargesGatewayTests
    {
        private readonly Mock<IAmazonDynamoDB> _amazonDynamoDB = new Mock<IAmazonDynamoDB>();
        private ChargesGateway _sut;

        public ChargesGatewayTests()
        {
            _sut = new ChargesGateway(_amazonDynamoDB.Object);
        }

        [Fact]
        public void GetAllByAssetIdWithEmptyInputReturnsException()
        {
            Func<Task<List<Charge>>> func = async () => await _sut.GetAllByAssetId(Guid.Empty).ConfigureAwait(false);
            func.Should().Throw<ArgumentNullException>();
        }
    }
}
