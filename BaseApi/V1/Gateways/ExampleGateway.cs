using System;
using System.Collections.Generic;
using BaseApi.V1.Domain;
using BaseApi.V1.Domain.SuspenseTransaction;
using BaseApi.V1.Factories;
using BaseApi.V1.Infrastructure;

namespace BaseApi.V1.Gateways
{
    //TODO: Rename to match the data source that is being accessed in the gateway eg. MosaicGateway
    public class ExampleGateway : IExampleGateway
    {
        private readonly DatabaseContext _databaseContext;

        public ExampleGateway(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public ConfirmTransferEntity GetEntityById(Guid id)
        {
            var result = _databaseContext.DatabaseEntities.Find(id);

            return result?.ToDomain();
        }

        public List<ConfirmTransferEntity> GetAll()
        {
            return new List<ConfirmTransferEntity>();
        }
    }
}
