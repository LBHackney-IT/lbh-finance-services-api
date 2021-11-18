using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Domain.ContactDetails;

namespace FinanceServicesApi.V1.UseCase.Interfaces
{
    public interface IGetContactDetailsByTargetIdUseCase
    {
        public Task<List<ContactDetails>> ExecuteAsync(Guid targetId);
    }
}
