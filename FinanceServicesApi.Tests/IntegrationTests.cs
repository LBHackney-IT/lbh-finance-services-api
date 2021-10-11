using System;
using System.Net.Http;
using FinanceServicesApi.V1.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;

namespace FinanceServicesApi.Tests
{
    public class IntegrationTests<TStartup> : IDisposable where TStartup : class
    {
        public IntegrationTests()
        {

        }

        public void Dispose()
        {

        }
    }
}
