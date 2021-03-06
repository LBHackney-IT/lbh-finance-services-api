using FinanceServicesApi.V1.Domain.Charges;
using FinanceServicesApi.V1.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using FinanceServicesApi.V1.Infrastructure.Enums;

namespace FinanceServicesApi.V1.Infrastructure.UrlGenerators
{
    public class ChargesUrlGenerator : IGenerateUrl<List<Charge>>
    {
        private readonly IGetEnvironmentVariables<List<Charge>> _getEnvironmentVariables;

        public ChargesUrlGenerator(IGetEnvironmentVariables<List<Charge>> getEnvironmentVariables)
        {
            _getEnvironmentVariables = getEnvironmentVariables;
        }

        public Uri Execute(Guid id, SearchBy searchBy = SearchBy.ById)
        {
            var url = _getEnvironmentVariables.GetUrl();
            return new Uri($"{url}/charges?targetId={id}");
        }
    }
}
