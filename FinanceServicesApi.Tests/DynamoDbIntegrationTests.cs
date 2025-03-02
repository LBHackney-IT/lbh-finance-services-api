using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace FinanceServicesApi.Tests
{
    public class DynamoDbIntegrationTests<TStartup> : IDisposable where TStartup : class
    {
        protected HttpClient Client { get; private set; }
        private DynamoDbMockWebApplicationFactory<TStartup> _factory;
        protected IDynamoDBContext DynamoDbContext => _factory?.DynamoDbContext;
        protected List<Action> CleanupActions { get; set; }

        public DynamoDbIntegrationTests()
        {
            EnsureEnvVarConfigured("DynamoDb_LocalMode", "true");
            EnsureEnvVarConfigured("DynamoDb_LocalServiceUrl", "http://localhost:8000");
            EnsureEnvVarConfigured("AWS_REGION", "eu-west-2");
            EnsureEnvVarConfigured("AWS_ACCESS_KEY_ID", "local");
            EnsureEnvVarConfigured("AWS_SECRET_ACCESS_KEY", "local");

            _factory = new DynamoDbMockWebApplicationFactory<TStartup>(_tables);
            Client = _factory.CreateClient();
            CleanupActions = new List<Action>();
        }

        private readonly List<TableDef> _tables = new List<TableDef>
        {
            // TODO: Populate the list of table(s) and their key property details here, for example:
            //new TableDef { Name = "example_table", KeyName = "id", KeyType = ScalarAttributeType.N }
        };

        private static void EnsureEnvVarConfigured(string name, string defaultValue)
        {
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(name)))
                Environment.SetEnvironmentVariable(name, defaultValue);
        }

        public void Dispose()
        {
            _factory.Dispose();
            foreach (var act in CleanupActions)
                act();
            Client.Dispose();
        }
    }

    public class TableDef
    {
        public string Name { get; set; }
        public string KeyName { get; set; }
        public ScalarAttributeType KeyType { get; set; }
    }
}
