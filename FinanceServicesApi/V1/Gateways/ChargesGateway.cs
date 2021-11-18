using System;
using System.ComponentModel;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Domain.Charges;
using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.Infrastructure.Interfaces;
using Newtonsoft.Json;

namespace FinanceServicesApi.V1.Gateways
{
    public class ChargesGateway : IChargesGateway
    {
        private readonly ICustomeHttpClient _client;
        private readonly IGetEnvironmentVariables _getEnvironmentVariables;

        public ChargesGateway(ICustomeHttpClient client, IGetEnvironmentVariables getEnvironmentVariables)
        {
            _client = client;
            _getEnvironmentVariables = getEnvironmentVariables;
        }
        public async Task<Charge> GetAllByTargetId(Guid targetId, TargetType targetType)
        {
            if (!Enum.IsDefined(typeof(TargetType), targetType))
                throw new InvalidEnumArgumentException(nameof(targetType), (int) targetType, typeof(TargetType));

            if (targetId == Guid.Empty)
                throw new ArgumentNullException($"the {nameof(targetId).ToString()} shouldn't be empty or null");

            var chargesApiUrl = _getEnvironmentVariables.GetChargesApiUrl().ToString();
            var chargesApiKey = _getEnvironmentVariables.GetChargesApiKey();

            _client.AddHeader(new HttpHeader<string, string> { Name = "x-api-key", Value = chargesApiKey });

            var response = await _client.GetAsync(new Uri($"{chargesApiUrl}/api/v1/charges?type={targetType}&targetId={targetId.ToString()}")).ConfigureAwait(false);
            if (response == null)
            {
                throw new Exception("The charges api is not reachable!");
            }
            else if (response.Content == null)
            {
                throw new Exception(response.StatusCode.ToString());
            }
            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var chargesResponse = JsonConvert.DeserializeObject<Charge>(responseContent);
            return chargesResponse;

        }
    }
}
