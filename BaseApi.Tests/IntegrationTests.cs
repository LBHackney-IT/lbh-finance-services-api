using System;
using System.Net.Http;
using BaseApi.V1.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;

namespace BaseApi.Tests
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
