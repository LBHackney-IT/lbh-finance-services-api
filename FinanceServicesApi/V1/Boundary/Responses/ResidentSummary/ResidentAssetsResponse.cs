using FinanceServicesApi.V1.Domain.TenureModels;
using Hackney.Shared.Asset.Domain;
using Hackney.Shared.Tenure.Domain;

namespace FinanceServicesApi.V1.Boundary.Responses.ResidentSummary
{
    public class ResidentAssetsResponse
    {
        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     E8 1ND
        /// </example>
        public AssetAddress AssetAddress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     Dwelling
        /// </example>
        public AssetType? AssetType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     HRA SEC
        /// </example>
        public TenureType TenancyType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     1234.56
        /// </example>
        public float? CurrentBalance { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     123.45
        /// </example>
        public float? ServiceCharge { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     1234.5
        /// </example>
        public float? RentCharge { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     25
        /// </example>
        public int? Staircasting { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <example>
        ///     123456789
        /// </example>
        public string RentAccountNumber { get; set; }
        public TenurePartialModel Tenure { get; set; }
    }
}
