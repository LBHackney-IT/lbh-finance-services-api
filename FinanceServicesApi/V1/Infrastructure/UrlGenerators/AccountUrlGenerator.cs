using FinanceServicesApi.V1.Domain.AccountModels;
using FinanceServicesApi.V1.Infrastructure.Interfaces;
using System;

namespace FinanceServicesApi.V1.Infrastructure.UrlGenerators
{
    public class AccountUrlGenerator : IGenerateUrl<Account>
    {
        private readonly IGetEnvironmentVariables<Account> _getEnvironmentVariables;

        public AccountUrlGenerator(IGetEnvironmentVariables<Account> getEnvironmentVariables)
        {
            _getEnvironmentVariables = getEnvironmentVariables;
        }
        public Uri Execute(Guid id)
        {
            var url = _getEnvironmentVariables.GetUrl();
            return new Uri($"{url}/accounts/{id}");
        }
    }
}
