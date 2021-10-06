using System.Collections.Generic;
using System.Linq;
using BaseApi.V1.Boundary.Response;
using BaseApi.V1.Domain;
using BaseApi.V1.Domain.SuspenseTransaction;

namespace BaseApi.V1.Factories
{
    public static class ResponseFactory
    {
        //TODO: Map the fields in the domain object(s) to fields in the response object(s).
        // More information on this can be found here https://github.com/LBHackney-IT/lbh-base-api/wiki/Factory-object-mappings
        /*public static ResponseObject ToResponse(this ConfirmTransferEntity domain)
        {
            return new ResponseObject();
        }

        public static List<ResponseObject> ToResponse(this IEnumerable<ConfirmTransferEntity> domainList)
        {
            return domainList.Select(domain => domain.ToResponse()).ToList();
        }*/
    }
}
