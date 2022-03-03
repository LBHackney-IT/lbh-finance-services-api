using System;
using FinanceServicesApi.V1.Infrastructure.Enums;
using FinanceServicesApi.V1.Infrastructure.Interfaces;
using Hackney.Shared.Person;

namespace FinanceServicesApi.V1.Infrastructure.UrlGenerators
{
    public class PersonUrlGenerator : IGenerateUrl<Person>
    {
        private readonly IGetEnvironmentVariables<Person> _getEnvironmentVariables;

        public PersonUrlGenerator(IGetEnvironmentVariables<Person> getEnvironmentVariables)
        {
            _getEnvironmentVariables = getEnvironmentVariables;
        }

        public Uri Execute(Guid id, SearchBy searchBy = SearchBy.ById)
        {
            var url = _getEnvironmentVariables.GetUrl();
            return new Uri($"{url}/persons/{id}");
        }
    }
}
