using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Domain.FinancialSummary;
using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace FinanceServicesApi.V1.Gateways
{
    public class FinancialSummaryGateway : IFinancialSummaryGateway
    {
        private readonly ICustomeHttpClient _client;
        private readonly IGetEnvironmentVariables _getEnvironmentVariables;

        public FinancialSummaryGateway(ICustomeHttpClient client, IGetEnvironmentVariables getEnvironmentVariables)
        {
            _client = client;
            _getEnvironmentVariables = getEnvironmentVariables;
        }
        public async Task<List<WeeklySummary>> GetGetAllByTargetId(Guid targetId, DateTime? startDate, DateTime? endDate)
        {
            if (targetId == Guid.Empty)
                throw new ArgumentNullException($"the {nameof(targetId).ToString()} shouldn't be empty or null");

            var financialSummaryApiUrl = _getEnvironmentVariables.GetFinancialSummaryApiUrl();
            var financialSummaryApiKey = _getEnvironmentVariables.GetFinancialSummaryApiKey();

            _client.AddHeader(new HttpHeader<string, string> { Name = "x-api-key", Value = financialSummaryApiKey });

            var response = await _client.GetAsync(new Uri($"{financialSummaryApiUrl}/api/v1/weekly-summary?targetid={targetId.ToString()}&startDate={startDate.ToString()}&endDate={endDate.ToString()}")).ConfigureAwait(false);
            if (response == null)
            {
                throw new Exception("The financial summary api is not reachable!");
            }
            else if (response.Content == null)
            {
                throw new Exception(response.StatusCode.ToString());
            }
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var fsResponse = JsonConvert.DeserializeObject<List<WeeklySummary>>(responseContent);
            return fsResponse;
        }
    }
}
