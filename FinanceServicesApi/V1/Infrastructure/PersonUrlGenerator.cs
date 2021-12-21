using System;
using FinanceServicesApi.V1.Infrastructure.Interfaces;
using Hackney.Shared.Person;

namespace FinanceServicesApi.V1.Infrastructure
{
    public class PersonUrlGenerator : IGenerateUrl<Person>
    {
        private readonly IGetEnvironmentVariables<Person> _getEnvironmentVariables;

        public PersonUrlGenerator(IGetEnvironmentVariables<Person> getEnvironmentVariables)
        {
            _getEnvironmentVariables = getEnvironmentVariables;
        }

        public Uri Execute(Guid id)
        {
            var url = _getEnvironmentVariables.GetUrl();
            return new Uri($"{url}/persons/{id}");
        }
    }
}
