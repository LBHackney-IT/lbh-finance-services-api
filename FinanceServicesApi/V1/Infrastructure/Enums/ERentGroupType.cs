using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FinanceServicesApi.V1.Infrastructure.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RentGroupType
    {
        Tenant,
        LeaseHolders,
        GenFundRents,
        Garages,
        HaLeases,
        HraRents,
        MajorWorks,
        TempAcc,
        Travelers,
        GarParkHRA,
        HousingGenFund,
        HousingRevenue,
        LHMajorWorks,
        LHServCharges,
        RSLandXBorough,
        TempAccGenFun,
        TempAccomHRA,
        TravelGenFund
    }
}
