using System.ComponentModel.DataAnnotations;
using FinanceServicesApi.V1.Domain;
using FinanceServicesApi.V1.Infrastructure;
using FluentAssertions;
using Xunit;

namespace FinanceServicesApi.Tests.V1.Infrastructure
{
    public class AllowedValuesAttributeTests
    {
        private AllowedValuesAttribute _allowedValues;

        [Theory]
        [InlineData(AccountType.Recharge)]
        [InlineData(TargetType.Tenure)]
        public void IsValidEnumTypeEntryReturnsSuccess<T>(T enmType)
        {
            _allowedValues = new AllowedValuesAttribute(typeof(T));
            _allowedValues.GetValidationResult(enmType, new ValidationContext(this)).Should().BeEquivalentTo(ValidationResult.Success);
        }

        [Theory]
        [InlineData(123)]
        [InlineData(null)]
        [InlineData("123")]
        [InlineData(true)]
        public void IsValidEnumTypeEntryReturnsThrowValidationException<T>(T data)
        {
            _allowedValues = new AllowedValuesAttribute(typeof(T));
            void Func() => _allowedValues.Validate(data, new ValidationContext(this));

            Assert.Throws<ValidationException>(Func);

        }
    }
}
