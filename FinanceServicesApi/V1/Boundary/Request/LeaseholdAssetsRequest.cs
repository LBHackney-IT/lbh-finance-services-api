using FinanceServicesApi.V1.Boundary.Request.MetaData;


namespace FinanceServicesApi.V1.Boundary.Request
{
    public class LeaseholdAssetsRequest : HousingSearchRequest
    {
        public short Year { get; set; }
    }
}
