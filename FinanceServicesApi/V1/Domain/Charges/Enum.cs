using System.Text.Json.Serialization;

namespace FinanceServicesApi.V1.Domain.Charges
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TargetType
    {
        Asset,
        Tenure,
        Block,
        Estate
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ChargeGroup
    {
        Tenants,
        Leaseholders
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ChargeType
    {
        Estate,
        Block,
        Property
    }
}
