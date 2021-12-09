using System.Collections.Generic;
using FinanceServicesApi.V1.Domain.ContactDetails;

namespace FinanceServicesApi.V1.Boundary.Responses
{
    public class GetContactDetailsResponse
    {
        public List<ContactDetail> Results { get; set; }
    }
}
