using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace BaseApi.V1.Infrastructure
{
    public static class ModelStateExtension
    {
        public static string GetErrorMessages(this ModelStateDictionary modelState)
        {
            return
                string.Join(",", modelState.SelectMany(e => e.Value.Errors.Select(s => s.ErrorMessage)));
        }
    }
}
