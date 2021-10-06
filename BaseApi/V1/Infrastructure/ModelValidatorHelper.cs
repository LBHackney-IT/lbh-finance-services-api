using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BaseApi.V1.Infrastructure
{
    public static class ModelValidatorHelper
    {
        /// <summary>
        /// Validate the model and return a response, which includes any validation messages and an IsValid bit.
        /// </summary>
        public static ValidationResponse Validate(object model)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(model);

            var isValid = Validator.TryValidateObject(model, context, results, true);

            return new ValidationResponse()
            {
                IsValid = isValid,
                Results = results
            };
        }

        /// <summary>
        /// Validate the model and return a bit indicating whether the model is valid or not.
        /// </summary>
        public static bool IsModelValid(object model)
        {
            var response = Validate(model);

            return response.IsValid;
        }
    }
     

    public class ValidationResponse
    {
        public List<ValidationResult> Results { get; set; }
        public bool IsValid { get; set; }

        public ValidationResponse()
        {
            Results = new List<ValidationResult>();
            IsValid = false;
        }
    }
}
