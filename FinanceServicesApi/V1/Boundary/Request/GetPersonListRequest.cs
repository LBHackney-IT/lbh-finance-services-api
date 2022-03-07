using FinanceServicesApi.V1.Boundary.Request.MetaData;
using Hackney.Shared.HousingSearch.Domain;
using Microsoft.AspNetCore.Mvc;

namespace FinanceServicesApi.V1.Boundary.Request
{
    public class GetPersonListRequest : HousingSearchRequest
    {
        [FromQuery(Name = "personType")]
        public PersonType? PersonType { get; set; }
    }
}
