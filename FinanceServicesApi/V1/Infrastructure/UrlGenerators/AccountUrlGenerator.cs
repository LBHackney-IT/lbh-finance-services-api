using FinanceServicesApi.V1.Domain.AccountModels;
using FinanceServicesApi.V1.Infrastructure.Interfaces;
using System;
using FinanceServicesApi.V1.Infrastructure.Enums;

namespace FinanceServicesApi.V1.Infrastructure.UrlGenerators
{
    public class AccountUrlGenerator : IGenerateUrl<Account>
    {
        private readonly IGetEnvironmentVariables<Account> _getEnvironmentVariables;

        public AccountUrlGenerator(IGetEnvironmentVariables<Account> getEnvironmentVariables)
        {
            _getEnvironmentVariables = getEnvironmentVariables;
        }

        public Uri Execute(Guid id, SearchBy searchBy = SearchBy.ById)
        {
            switch (searchBy)
            {
                case SearchBy.ById:
                    {
                        var url = _getEnvironmentVariables.GetUrl();
                        return new Uri($"{url}/accounts/{id}");
                    }
                case SearchBy.ByTargetId:
                    {
                        var url = _getEnvironmentVariables.GetUrl();
                        return new Uri($"{url}/accounts?targetId={id}");
                    }
                default:
                    throw new ArgumentException($"{nameof(searchBy)} is invalid.");
            }
        }
    }
}
