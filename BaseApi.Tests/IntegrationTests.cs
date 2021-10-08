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
        protected HttpClient Client { get; private set; }

        private MockWebApplicationFactory<TStartup> _factory;
        private NpgsqlConnection _connection;
        private IDbContextTransaction _transaction;
        private DbContextOptionsBuilder _builder;

        public IntegrationTests()
        {
            _connection = new NpgsqlConnection(ConnectionString.TestDatabase());
            _connection.Open();
            var npgsqlCommand = _connection.CreateCommand();
            npgsqlCommand.CommandText = "SET deadlock_timeout TO 30";
            npgsqlCommand.ExecuteNonQuery();

            _builder = new DbContextOptionsBuilder();
            _builder.UseNpgsql(_connection);

            _factory = new MockWebApplicationFactory<TStartup>(_connection);
            Client = _factory.CreateClient(); 
        }

        public void Dispose()
        {
            Client?.Dispose();
            _factory?.Dispose();
            _transaction.Rollback();
            _transaction?.Dispose();
            _connection?.Dispose(); 
        }
    }
}
