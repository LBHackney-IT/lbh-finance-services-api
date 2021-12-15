using System;
using Amazon.DynamoDBv2.Model;
using FinanceServicesApi.Tests.V1.Helper;
using FinanceServicesApi.V1.Domain.Charges;
using FinanceServicesApi.V1.Domain.TransactionModels;
using FinanceServicesApi.V1.Infrastructure;
using FluentAssertions;
using Xunit;

namespace FinanceServicesApi.Tests.V1.Infrastructure
{
    public class QueryResponseExtensionTests
    {
        [Fact]
        public void ToChargesWithCorrespondingDataTypeReturnsListOfCharges()
        {
            QueryResponse response = FakeDataHelper.MockQueryResponse<Charge>(5);
            var result = response.ToCharge();
            result.Should().AllBeOfType<Charge>();
        }
        [Fact]
        public void ToTransactionWithCorrespondingDataTypeReturnsListOfTransaction()
        {
            QueryResponse response = FakeDataHelper.MockQueryResponse<Transaction>(5);
            var result = response.ToTransactions();
            result.Should().AllBeOfType<Transaction>();
        }

        [Fact]
        public void ToDomainWithNonCorrespondingDataTypeThrowsException()
        {
            QueryResponse response = FakeDataHelper.MockQueryResponse<Transaction>(5);
            Assert.Throws<System.Collections.Generic.KeyNotFoundException>(() => response.ToCharge());
        }
    }
}
