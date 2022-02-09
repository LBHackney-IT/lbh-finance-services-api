using FinanceServicesApi.V1.Boundary.Responses;
using FinanceServicesApi.V1.Infrastructure.Interfaces;
using System;

namespace FinanceServicesApi.V1.Infrastructure.UrlGenerators
{
    public class ContactDetailUrlGenerator : IGenerateUrl<GetContactDetailsResponse>
    {
        private readonly IGetEnvironmentVariables<GetContactDetailsResponse> _getEnvironmentVariables;

        public ContactDetailUrlGenerator(IGetEnvironmentVariables<GetContactDetailsResponse> getEnvironmentVariables)
        {
            _getEnvironmentVariables = getEnvironmentVariables;
        }
        public Uri Execute(Guid id)
        {
            var url = _getEnvironmentVariables.GetUrl();
            return new Uri($"{url}/contactDetails?TargetId={id}");
        }
    }
}
