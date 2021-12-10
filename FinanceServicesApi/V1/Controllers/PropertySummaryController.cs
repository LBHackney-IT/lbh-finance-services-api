using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceServicesApi.V1.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/v1/property-summary")]
    [ApiVersion("1.0")]
    public class PropertySummaryController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
