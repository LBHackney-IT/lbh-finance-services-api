using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Domain.ContactDetails;
using FinanceServicesApi.V1.Gateways.Interfaces;
using FinanceServicesApi.V1.UseCase.Interfaces;

namespace FinanceServicesApi.V1.UseCase
{
    public class GetContactDetailsByTargetIdUseCase : IGetContactDetailsByTargetIdUseCase
    {
        private readonly IContactDetailsGateway _gateway;

        public GetContactDetailsByTargetIdUseCase(IContactDetailsGateway gateway)
        {
            _gateway = gateway;
        }
        public async Task<List<ContactDetail>> ExecuteAsync(Guid targetId)
        {
            return await _gateway.GetByTargetId(targetId).ConfigureAwait(false);
        }
    }
}
