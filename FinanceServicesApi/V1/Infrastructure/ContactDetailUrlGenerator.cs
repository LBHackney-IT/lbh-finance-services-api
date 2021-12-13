using System;
using System.Collections.Generic;
using FinanceServicesApi.V1.Boundary.Responses;
using FinanceServicesApi.V1.Domain.ContactDetails;
using FinanceServicesApi.V1.Infrastructure.Interfaces;

namespace FinanceServicesApi.V1.Infrastructure
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
