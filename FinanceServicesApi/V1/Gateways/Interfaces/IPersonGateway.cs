using System;
using System.Threading.Tasks;
using Hackney.Shared.Person;

namespace FinanceServicesApi.V1.Gateways.Interfaces
{
    public interface IPersonGateway
    {
        public Task<Person> GetById(Guid id);
    }
}
