using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Util;
using FinanceServicesApi.V1.Domain.Charges;
using FinanceServicesApi.V1.Domain.FinancialSummary;
using FinanceServicesApi.V1.Factories;
using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.Infrastructure;
using FinanceServicesApi.V1.Infrastructure.Entities;
using FinanceServicesApi.V1.Infrastructure.Enums;
using FinanceServicesApi.V1.Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace FinanceServicesApi.V1.Gateways
{
    public class FinancialSummaryByTargetIdGateway : IFinancialSummaryByTargetIdGateway
    {
        private const string Targetid = "target_id";
        public string PaginationToken { get; set; } = "{}";

        private readonly IAmazonDynamoDB _amazonDynamoDb;
        private readonly IDynamoDBContext _dynamoDbContext;

        public FinancialSummaryByTargetIdGateway(IAmazonDynamoDB amazonDynamoDb, IDynamoDBContext dynamoDbContext)
        {
            _amazonDynamoDb = amazonDynamoDb;
            _dynamoDbContext = dynamoDbContext;
        }
        public async Task<List<WeeklySummary>> GetGetAllByTargetId(Guid targetId, DateTime? startDate, DateTime? endDate)
        {
            if (targetId == Guid.Empty)
                throw new ArgumentNullException($"the {nameof(targetId).ToString()} shouldn't be empty or null");

            var dbWeeklySummary = new List<WeeklySummaryDbEntity>();
            var table = _dynamoDbContext.GetTargetTable<WeeklySummaryDbEntity>();

            var queryConfig = new QueryOperationConfig
            {
                BackwardSearch = true,
                ConsistentRead = true,
                Filter = new QueryFilter(Targetid, QueryOperator.Equal, targetId),
                PaginationToken = PaginationToken
            };
            queryConfig.Filter.AddCondition("summary_type", QueryOperator.Equal, SummaryType.WeeklySummary.ToString());
            if (startDate.HasValue && endDate.HasValue)
            {
                queryConfig.Filter.AddCondition("submit_date", QueryOperator.Between, startDate.Value.ToString(AWSSDKUtils.ISO8601DateFormat), endDate.Value.ToString(AWSSDKUtils.ISO8601DateFormat));
            }

            do
            {
                var search = table.Query(queryConfig);
                PaginationToken = search.PaginationToken;
                var resultsSet = await search.GetNextSetAsync().ConfigureAwait(false);
                if (resultsSet.Any())
                {
                    dbWeeklySummary.AddRange(_dynamoDbContext.FromDocuments<WeeklySummaryDbEntity>(resultsSet));

                }
            }
            while (!string.Equals(PaginationToken, "{}", StringComparison.Ordinal));

            return dbWeeklySummary.ToDomain();

            /*QueryRequest request = new QueryRequest
            {
                TableName = "FinancialSummaries",
                *//*IndexName = "target_id",*//*
                KeyConditionExpression = "target_id = :V_target_id",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {":V_target_id",new AttributeValue{S = targetId.ToString()}}
                },
                ScanIndexForward = true
            };

            var response = await _amazonDynamoDb.QueryAsync(request).ConfigureAwait(false);
            return response.ToWeeklySummary();*/
        }
    }
}
