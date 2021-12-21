using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceServicesApi.V1.Domain.TransactionModels;
using FinanceServicesApi.V1.UseCase.Interfaces;

namespace FinanceServicesApi.V1.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/v1/dummy/transaction")]
    [ApiVersion("1.0")]
    public class DummyController : BaseController
    {
        private readonly IGetTenureInformationByIdUseCase _tenure;
        private readonly
        public DummyController(IGetTenureInformationByIdUseCase tenure)
        {
            _tenure = tenure;
        }

        public async Task<IActionResult> GetTransaction(Guid id)
        {

            var tenure = await _tenure.ExecuteAsync(id).ConfigureAwait(false);

        }
    }
}
