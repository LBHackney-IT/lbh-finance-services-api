using FinanceServicesApi.V1.Infrastructure.Interfaces;
using System;
using FinanceServicesApi.V1.Boundary.Responses;
using FinanceServicesApi.V1.Infrastructure.Enums;

namespace FinanceServicesApi.V1.Infrastructure.UrlGenerators
{
    public class AccountsUrlGenerator : IGenerateUrl<GetAccountListResponse>
    {
        private readonly IGetEnvironmentVariables<GetAccountListResponse> _getEnvironmentVariables;

        public AccountsUrlGenerator(IGetEnvironmentVariables<GetAccountListResponse> getEnvironmentVariables)
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
