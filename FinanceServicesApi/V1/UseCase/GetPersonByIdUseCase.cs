using System;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.UseCase.Interfaces;
using Hackney.Shared.Person;

namespace FinanceServicesApi.V1.UseCase
{
    public class GetPersonByIdUseCase: IGetPersonByIdUseCase
    {
        private readonly IPersonGateway _gateway;

        public GetPersonByIdUseCase(IPersonGateway gateway)
        {
            _gateway = gateway;
        }
        public async Task<Person> ExecuteAsync(Guid id)
        {
            return await _gateway.GetById(id).ConfigureAwait(false);
        }
    }
}
