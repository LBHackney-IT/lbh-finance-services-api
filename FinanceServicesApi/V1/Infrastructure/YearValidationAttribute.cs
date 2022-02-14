using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace FinanceServicesApi.V1.Infrastructure
{
    public class YearValidationAttribute : ValidationAttribute
    {
        private readonly short _minimalYear;

        public YearValidationAttribute(short minimalYear)
        {
            _minimalYear = minimalYear;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is short year)
            {
                return _minimalYear <= year
                    && year <= DateTime.UtcNow.Year
                     ? ValidationResult.Success
                     : new ValidationResult($"Year ({year}) should be between {_minimalYear} - {DateTime.UtcNow.Year}");
            }

            return new ValidationResult("Type error");
        }
    }
}
