using System;
using System.Collections.Generic;
using BaseApi.V1.Domain;
using BaseApi.V1.Domain.SuspenseTransaction;

namespace BaseApi.V1.Gateways
{
    public interface IExampleGateway
    {
        ConfirmTransferEntity GetEntityById(Guid id);

        List<ConfirmTransferEntity> GetAll();
    }
}
