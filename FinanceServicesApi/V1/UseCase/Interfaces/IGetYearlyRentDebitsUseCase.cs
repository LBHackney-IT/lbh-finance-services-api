using FinanceServicesApi.V1.Boundary.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanceServicesApi.V1.UseCase.Interfaces
{
    public interface IGetYearlyRentDebitsUseCase
    {
        public Task<List<YearlyRentDebitResponse>> ExecuteAsync(Guid assetId);
    }
}
