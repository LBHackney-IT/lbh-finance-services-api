using System.Text.Json.Serialization;

namespace FinanceServicesApi.V1.Infrastructure.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SubType
    {
        CorrespondenceAddress,
        Mobile,
        Home,
        Work,
        Other,
        Landline
    }
}
