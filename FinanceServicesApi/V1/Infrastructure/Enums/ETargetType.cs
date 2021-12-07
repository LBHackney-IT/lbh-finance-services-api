using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FinanceServicesApi.V1.Infrastructure.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TargetType
    {
        Tenure
    }
}
