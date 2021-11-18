using System.Text.Json.Serialization;

namespace FinanceServicesApi.V1.Domain.ContactDetails
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

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ContactType
    {
        Phone,
        Email,
        Address
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TargetType
    {
        Person,
        Organisation
    }
}
