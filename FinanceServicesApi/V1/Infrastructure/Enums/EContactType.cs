using System.Text.Json.Serialization;

namespace FinanceServicesApi.V1.Infrastructure.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ContactType
    {
        Phone,
        Email,
        Address
    }
}
