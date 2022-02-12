using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FinanceServicesApi.V1.Boundary.Responses.PropertySummary
{
    // Hanna Holasava
    // Once the PR will be merged, we will use Enum from FinanceServicesApi.V1.Boundary.Request.Enums namespace
    // https://github.com/LBHackney-IT/lbh-finance-services-api/pull/64
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AssetType
    {
        Property,
        Block,
        Estate
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ChargeSubGroup
    {
        Actual,
        Estimate
    }

    public class AssetAppointmentResponse
    {
        public Guid AssetId { get; set; }

        public List<ChargesTotalResponse> EstateCostTotal { get; set; }
        public List<ChargesTotalResponse> BlockCostTotal { get; set; }
        public List<ChargesTotalResponse> PropertyCostTotal { get; set; }

        public string EstateName { get; set; }
        public List<PropertyCostTotals> EstateCosts { get; set; }

        public string BlockName { get; set; }
        public List<PropertyCostTotals> BlockCosts { get; set; }

        public List<PropertyCostTotals> PropertyCosts { get; set; }
    }

    // Hanna Holasava
    // Once the PR will be merged, we will use Enum from FinanceServicesApi.V1.Boundary.Response namespace
    // https://github.com/LBHackney-IT/lbh-finance-services-api/pull/64
    public class ChargesTotalResponse
    {
        ///<example>50.5</example>
        public decimal Amount { get; set; }

        ///<example>2021</example>
        public short Year { get; set; }

        ///<example>Actual</example>
        public ChargeSubGroup Type { get; set; }
    }
}
