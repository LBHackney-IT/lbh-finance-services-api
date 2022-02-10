using FinanceServicesApi.V1.Domain.AccountModels;
using FinanceServicesApi.V1.Infrastructure.Interfaces;
using System;

namespace FinanceServicesApi.V1.Infrastructure.UrlGenerators
{
    public class AccountTargetUrlGenerator : IGenerateUrl<Account>
    {
        private readonly IGetEnvironmentVariables<Account> _getEnvironmentVariables;

        public AccountTargetUrlGenerator(IGetEnvironmentVariables<Account> getEnvironmentVariables)
        {
            _getEnvironmentVariables = getEnvironmentVariables;
        }
        public Uri Execute(Guid id)
        {
            var url = _getEnvironmentVariables.GetUrl();
            return new Uri($"{url}/accounts?targetId={id}");
        }
    }
}
