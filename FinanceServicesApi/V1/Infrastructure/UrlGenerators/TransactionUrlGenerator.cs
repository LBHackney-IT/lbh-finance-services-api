using System;
using FinanceServicesApi.V1.Domain.TransactionModels;
using FinanceServicesApi.V1.Infrastructure.Enums;
using FinanceServicesApi.V1.Infrastructure.Interfaces;

namespace FinanceServicesApi.V1.Infrastructure.UrlGenerators
{
    public class TransactionUrlGenerator : IGenerateUrl<Transaction>
    {
        private readonly IGetEnvironmentVariables<Transaction> _getEnvironmentVariables;

        public TransactionUrlGenerator(IGetEnvironmentVariables<Transaction> getEnvironmentVariables)
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
                        return new Uri($"{url}/transactions/{Guid.Empty}?targetId={id}");
                    }
                case SearchBy.ByTargetId:
                    {
                        var url = _getEnvironmentVariables.GetUrl();
                        return new Uri($"{url}/transactions/{id}/tenureId");
                    }
                default:
                    throw new ArgumentException($"{nameof(searchBy)} is invalid.");
            }
        }
    }
}
