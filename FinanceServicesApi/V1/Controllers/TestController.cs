using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceServicesApi.V1.UseCase;
using FinanceServicesApi.V1.UseCase.Interfaces;

namespace FinanceServicesApi.V1.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/v1/test")]
    [ApiVersion("1.0")]
    public class TestController : Controller
    {
        private readonly IGetPersonByIdUseCase _personUseCase;
        private readonly IGetAssetByIdUseCase _assetUseCase;
        private readonly IGetTenureInformationByIdUseCase _tenureUseCase;
        private readonly IGetContactDetailsByTargetIdUseCase _contactUsecase;

        public TestController(IGetPersonByIdUseCase personUseCase, IGetAssetByIdUseCase assetUseCase, IGetTenureInformationByIdUseCase tenureUseCase, IGetContactDetailsByTargetIdUseCase contactUsecase)
        {
            _personUseCase = personUseCase;
            _assetUseCase = assetUseCase;
            _tenureUseCase = tenureUseCase;
            _contactUsecase = contactUsecase;
        }

        [HttpGet("person/{id}")]
        public async Task<IActionResult> GetPerson(Guid id)
        {
            try
            {
                var response = await _personUseCase.ExecuteAsync(id).ConfigureAwait(false);
                return Ok(response);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception exception)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                return Ok(exception.Message);
            }
        }

        [HttpGet("asset/{id}")]
        public async Task<IActionResult> GetAsset(Guid id)
        {
            try
            {
                var response = await _assetUseCase.ExecuteAsync(id).ConfigureAwait(false);
                return Ok(response);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception exception)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                return Ok(exception.Message);
            }
        }

        [HttpGet("tenure/{id}")]
        public async Task<IActionResult> GetTenure(Guid id)
        {
            try
            {
                var response = await _tenureUseCase.ExecuteAsync(id).ConfigureAwait(false);
                return Ok(response);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception exception)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                return Ok(exception.Message);
            }
        }

        [HttpGet("contact/{id}")]
        public async Task<IActionResult> GetContact(Guid id)
        {
            try
            {
                var response = await _contactUsecase.ExecuteAsync(id).ConfigureAwait(false);
                return Ok(response);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception exception)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                return Ok(exception.Message);
            }
        }
    }
}
