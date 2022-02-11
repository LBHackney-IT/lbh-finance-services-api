using System;
using System.Collections.Generic;
using FinanceServicesApi.V1.Domain.TransactionModels;
using FinanceServicesApi.V1.Infrastructure.Enums;
using FinanceServicesApi.V1.Infrastructure.Interfaces;

namespace FinanceServicesApi.V1.Infrastructure.UrlGenerators
{
    public class TransactionTargetIdUrlGenerator : IGenerateUrl<List<Transaction>>
    {
        private readonly IGetEnvironmentVariables<List<Transaction>> _getEnvironmentVariables;

        public TransactionTargetIdUrlGenerator(IGetEnvironmentVariables<List<Transaction>> getEnvironmentVariables)
        {
            _getEnvironmentVariables = getEnvironmentVariables;
        }

        /// <summary>
        /// This method is only for fetching the suspense transactions.
        /// </summary>
        /// <param name="id">Tenure id</param>
        /// <param name="searchBy"></param>
        /// <returns></returns>
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
