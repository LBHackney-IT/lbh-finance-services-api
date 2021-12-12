using System;
using System.ComponentModel.DataAnnotations;

namespace FinanceServicesApi.V1.Infrastructure
{
    public class GreatAndEqualThan : ValidationAttribute
    {
        private readonly decimal _minValue;

        public GreatAndEqualThan(string minValue)
        {
            if (decimal.TryParse(minValue, out var digit))
                _minValue = digit;
            else
                throw new Exception("Invalid input number");
        }
        public override bool IsValid(object value)
        {
            return (decimal) value >= _minValue;
        }
    }
}
