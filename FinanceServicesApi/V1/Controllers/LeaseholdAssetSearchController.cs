using FinanceServicesApi.V1.Boundary.Request;
using FinanceServicesApi.V1.Boundary.Request.MetaData;
using FinanceServicesApi.V1.Boundary.Responses;
using FinanceServicesApi.V1.Infrastructure;
using FinanceServicesApi.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FinanceServicesApi.V1.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/v1/leasehold-search")]
    [ApiVersion("1.0")]
    public class LeaseholdAssetSearchController : BaseController
    {
        private readonly IGetLeaseholdAssetsListUseCase _getLeaseholdAssetsListUseCase;

        public LeaseholdAssetSearchController(IGetLeaseholdAssetsListUseCase getLeaseholdAssetsListUseCase)
        {
            _getLeaseholdAssetsListUseCase = getLeaseholdAssetsListUseCase;
        }
        /// <summary>
        /// LeaseholdProperty Search
        /// </summary>
        /// <param name="housingSearchRequest">Search Request</param>
        /// <returns>Property Summary</returns>

        [ProducesResponseType(typeof(GetPropertyListResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(BaseErrorResponse), StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<IActionResult> GetLeaseholdAssetsByAddress([FromQuery] LeaseholdAssetsRequest housingSearchRequest)
        {
            if (housingSearchRequest == null)
                return BadRequest(new BaseErrorResponse((int) HttpStatusCode.BadRequest,
                    $"Search request cannot be empty."));

            var response = await _getLeaseholdAssetsListUseCase.ExecuteAsync(housingSearchRequest).ConfigureAwait(false);
            if (response != null)
            {
                return Ok(response);
            }
            else
                return BadRequest(new BaseErrorResponse((int) HttpStatusCode.BadRequest,
                    $"No match found"));
        }

    }
}
