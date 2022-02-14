using FinanceServicesApi.V1.Boundary.Request.Enums;
using FinanceServicesApi.V1.Boundary.Request.MetaData;
using FinanceServicesApi.V1.Infrastructure;

namespace FinanceServicesApi.V1.Boundary.Request
{
    public class LeaseholdAssetsRequest : HousingSearchRequest
    {
        [YearValidation(1970)]
        public short FromYear { get; set; }

        /// <summary>
        /// Property, Block, Estate
        /// </summary>
        public AssetType AssetType { get; set; }
    }
}
