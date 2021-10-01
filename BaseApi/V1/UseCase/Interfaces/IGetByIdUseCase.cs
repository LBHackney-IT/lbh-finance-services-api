using System;
using BaseApi.V1.Boundary.Response;

namespace BaseApi.V1.UseCase.Interfaces
{
    public interface IGetByIdUseCase
    {
        ResponseObject Execute(Guid id);
    }
}
