using System;
using FinanceServicesApi.V1.Infrastructure.Interfaces;
using Hackney.Shared.Tenure.Domain;

namespace FinanceServicesApi.V1.Infrastructure
{
    public class TenureUrlGenerator : IGenerateUrl<TenureInformation>
    {
        private readonly IGetEnvironmentVariables<TenureInformation> _getEnvironmentVariables;

        public TenureUrlGenerator(IGetEnvironmentVariables<TenureInformation> getEnvironmentVariables)
        {
            _getEnvironmentVariables = getEnvironmentVariables;
        }
        public Uri Execute(Guid id)
        {
            var url = _getEnvironmentVariables.GetUrl();
            return new Uri($"{url}/api/v1/tenures/{id.ToString()}");
        }
    }
}
