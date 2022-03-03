using System;
using FinanceServicesApi.V1.Infrastructure.Enums;
using FinanceServicesApi.V1.Infrastructure.Interfaces;
using Hackney.Shared.Tenure.Domain;

namespace FinanceServicesApi.V1.Infrastructure.UrlGenerators
{
    public class TenureUrlGenerator : IGenerateUrl<TenureInformation>
    {
        private readonly IGetEnvironmentVariables<TenureInformation> _getEnvironmentVariables;

        public TenureUrlGenerator(IGetEnvironmentVariables<TenureInformation> getEnvironmentVariables)
        {
            _getEnvironmentVariables = getEnvironmentVariables;
        }

        public Uri Execute(Guid id, SearchBy searchBy = SearchBy.ById)
        {
            var url = _getEnvironmentVariables.GetUrl();
            return new Uri($"{url}/tenures/{id.ToString()}");
        }
    }
}
