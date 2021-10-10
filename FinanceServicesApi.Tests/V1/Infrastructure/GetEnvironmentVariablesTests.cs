using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Infrastructure;
using FluentAssertions;
using Xunit;

namespace FinanceServicesApi.Tests.V1.Infrastructure
{
    public class GetEnvironmentVariablesTests
    {
        [Fact]
        public void GetAccountApiUrlExistsVariableReturnsValue()
        {
            GetEnvironmentVariables environmentVariables= new GetEnvironmentVariables();
            string result = environmentVariables.GetAccountApiUrl();
            result.Should().NotBeNullOrEmpty();
            result.Length.Should().BeGreaterThan(10);
        }

        [Fact]
        public void GetAccountApiTokenExistsVariableReturnsValue()
        {
            GetEnvironmentVariables environmentVariables = new GetEnvironmentVariables();
            string result = environmentVariables.GetAccountApiToken();
            result.Should().NotBeNullOrEmpty();
            result.Length.Should().BeGreaterThan(10);
        }

        [Fact]
        public void GetTransactionApiUrlExistsVariableReturnsValue()
        {
            GetEnvironmentVariables environmentVariables = new GetEnvironmentVariables();
            string result = environmentVariables.GetTransactionApiUrl();
            result.Should().NotBeNullOrEmpty();
            result.Length.Should().BeGreaterThan(10);
        }

        [Fact]
        public void GetTransactionApiKeyExistsVariableReturnsValue()
        {
            GetEnvironmentVariables environmentVariables = new GetEnvironmentVariables();
            string result = environmentVariables.GetTransactionApiKey();
            result.Should().NotBeNullOrEmpty();
            result.Length.Should().BeGreaterThan(10);
        }
    }
}
