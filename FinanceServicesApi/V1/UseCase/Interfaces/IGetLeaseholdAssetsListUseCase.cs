using FinanceServicesApi.V1.Boundary.Request;
using FinanceServicesApi.V1.Boundary.Request.MetaData;
using FinanceServicesApi.V1.Boundary.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceServicesApi.V1.UseCase.Interfaces
{
    public interface IGetLeaseholdAssetsListUseCase
    {
        Task<GetPropertyListResponse> ExecuteAsync(LeaseholdAssetsRequest housingSearchRequest);
    }
}
