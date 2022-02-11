using FinanceServicesApi.V1.Boundary.Request.Enums;

namespace FinanceServicesApi.V1.Infrastructure
{
    public static class EnumMapper
    {
        public static string ToHousingAssetType(this AssetType assetType)
        {
            switch (assetType)
            {
                case AssetType.Property: return "Dwelling";
                case AssetType.Block: return "Block";
                case AssetType.Estate: return "Estate";

                default: return string.Empty;
            }
        }
    }
}
