using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BaseApi.V1.Gateways.Interfaces.SuspenseTransaction.Account;
using BaseApi.V1.UseCase.Interfaces;
using IGetByIdUseCase = BaseApi.V1.UseCase.Interfaces.SuspenseTransaction.Accounts.IGetByIdUseCase;

namespace BaseApi.V1.Controllers
{
    public class SuspenseAccountController : BaseController
    {
        private readonly IGetByIdUseCase _getByIdUseCase;


        public SuspenseAccountController(IGetByIdUseCase getByIdUseCase)
        {
            _getByIdUseCase = getByIdUseCase;
        }


        [HttpGet]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var result = await _getByIdUseCase.ExecuteAsync(id).ConfigureAwait(false);
            return Ok(result);
        }

    }
}
