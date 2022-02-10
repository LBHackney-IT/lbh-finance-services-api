using FinanceServicesApi.V1.Boundary.Request.Enums;
using FinanceServicesApi.V1.Boundary.Request.MetaData;


namespace FinanceServicesApi.V1.Boundary.Request
{
    public class LeaseholdAssetsRequest : HousingSearchRequest
    {
        public short Year { get; set; }

        /// <summary>
        /// Property, Block, Estate
        /// </summary>
        public AssetType AssetType { get; set; }
    }
}
