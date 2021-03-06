using System;
using System.Threading.Tasks;
using Hackney.Shared.Person;

namespace FinanceServicesApi.V1.UseCase.Interfaces
{
    public interface IGetPersonByIdUseCase
    {
        public Task<Person> ExecuteAsync(Guid id);
    }
}
