using Amazon.Lambda.Core;
using FinanceServicesApi.V1.Boundary.Request;
using FinanceServicesApi.V1.Boundary.Responses;
using FinanceServicesApi.V1.Boundary.Responses.MetaData;
using FinanceServicesApi.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FinanceServicesApi.V1.Controllers
{
    /// <summary>
    /// Controller to work with Persons
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Route("api/v1/persons")]
    [ApiVersion("1.0")]
    public class PersonsController : BaseController
    {
        private readonly IGetPersonListUseCase _getPersonListUseCase;

        public PersonsController(IGetPersonListUseCase getPersonListUseCase)
        {
            _getPersonListUseCase = getPersonListUseCase;
        }

        /// <summary>
        /// Returns a list of Persons by seatchText
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(APIResponse<GetPersonListResponse>), 200)]
        [ProducesResponseType(typeof(APIResponse<string>), 400)]
        [HttpGet, MapToApiVersion("1")]
        public async Task<IActionResult> GetPersonList([FromQuery] GetPersonListRequest request)
        {
            try
            {
                var personsSearchResult = await _getPersonListUseCase.ExecuteAsync(request).ConfigureAwait(false);
                var apiResponse = new APIResponse<GetPersonListResponse>(personsSearchResult);
                apiResponse.Total = personsSearchResult.Total();

                return new OkObjectResult(apiResponse);
            }
            catch (Exception e)
            {
                LambdaLogger.Log(e.Message + e.StackTrace);
                return new BadRequestObjectResult(e.Message);
            }
        }
    }
}
