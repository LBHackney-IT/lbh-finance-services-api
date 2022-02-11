using System;
using FinanceServicesApi.V1.Infrastructure.Enums;
using FinanceServicesApi.V1.Infrastructure.Interfaces;
using Hackney.Shared.Asset.Domain;

namespace FinanceServicesApi.V1.Infrastructure.UrlGenerators
{
    public class AssetUrlGenerator : IGenerateUrl<Asset>
    {
        private readonly IGetEnvironmentVariables<Asset> _getEnvironmentVariables;

        public AssetUrlGenerator(IGetEnvironmentVariables<Asset> getEnvironmentVariables)
        {
            _getEnvironmentVariables = getEnvironmentVariables;
        }

        public Uri Execute(Guid id, SearchBy searchBy = SearchBy.ById)
        {
            var url = _getEnvironmentVariables.GetUrl();
            return new Uri($"{url}/assets/{id}");
        }
    }
}
