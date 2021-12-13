using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Domain.ContactDetails;

namespace FinanceServicesApi.V1.Gateways.Interfaces
{
    public interface IContactDetailsGateway
    {
        public Task<List<ContactDetail>> GetByTargetId(Guid targetId);

    }
}
