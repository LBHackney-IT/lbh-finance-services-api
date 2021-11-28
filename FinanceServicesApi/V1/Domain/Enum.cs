using System.Text.Json.Serialization;

namespace FinanceServicesApi.V1.Domain
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Title
    {
        Mr,
        Mrs,
        Ms,
        Dr
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RentGroupType
    {
        Tenant, LeaseHolders, GenFundRents, Garages, HaLeases, HraRents, MajorWorks, TempAcc, Travelers
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Direction
    {
        Asc,
        Desc
    }
}
