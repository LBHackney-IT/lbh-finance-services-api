using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Gateways;
using FinanceServicesApi.V1.UseCase.Interfaces;
using Hackney.Shared.Tenure.Domain;

namespace FinanceServicesApi.V1.UseCase
{
    public class GetTenureInformationByIdUseCase : IGetTenureInformationByIdUseCase
    {
        private readonly TenureInformationGateway _gateway;

        public GetTenureInformationByIdUseCase(TenureInformationGateway gateway)
        {
            _gateway = gateway;
        }
        public async Task<TenureInformation> ExecuteAsync(Guid id)
        {
            if (id == null)
                throw new Exception("The id shouldn't be empty or null.");
            return await _gateway.GetById(id).ConfigureAwait(false);
        }
    }
}
