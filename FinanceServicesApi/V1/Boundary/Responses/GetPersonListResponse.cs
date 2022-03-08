using Hackney.Shared.HousingSearch.Domain.Person;
using System.Collections.Generic;

namespace FinanceServicesApi.V1.Boundary.Responses
{
    public class GetPersonListResponse
    {
        private long _total;

        public List<Person> Persons { get; set; }

        public GetPersonListResponse()
        {
            Persons = new List<Person>();
        }

        public void SetTotal(long total)
        {
            _total = total < 0 ? 0 : total;
        }

        public long Total()
        {
            return _total;
        }
    }
}
