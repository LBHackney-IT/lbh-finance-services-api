using FinanceServicesApi.V1.Boundary.Request;
using FinanceServicesApi.V1.Boundary.Responses;
using System.Threading.Tasks;

namespace FinanceServicesApi.V1.UseCase.Interfaces
{
    public interface IGetPersonListUseCase
    {
        Task<GetPersonListResponse> ExecuteAsync(GetPersonListRequest housingSearchRequest);
    }
}
