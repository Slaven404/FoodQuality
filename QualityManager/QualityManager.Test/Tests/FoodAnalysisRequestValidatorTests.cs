using QualityManager.DTOs.Requests;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;
using QualityManager.Resources;

namespace QualityManager.Test.Tests
{
    public class FoodAnalysisRequestValidatorTests
    {
        [Fact]
        public void FoodAnalysisRequest_WithInvalidData_ShouldFailValidation()
        {
            var invalidRequest = new FoodBatchRequest
            {
                FoodName = "",
                SerialNumber = "12345678901234567890123456789055555",
                AnalysisTypeId = 0
            };

            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(invalidRequest, new ValidationContext(invalidRequest), validationResults, true);

            isValid.Should().BeFalse();
            validationResults.Should().HaveCount(3);
            validationResults.Should().Contain(x => x.ErrorMessage == nameof(Translations.Validation_FoodNameRequired));
            validationResults.Should().Contain(x => x.ErrorMessage == nameof(Translations.Validation_SerialNumberTooLong));
            validationResults.Should().Contain(x => x.ErrorMessage == nameof(Translations.Validation_AnalysisTypeMaxValue));
        }

        [Fact]
        public void FoodAnalysisRequest_WithValidData_ShouldPassValidation()
        {
            var validRequest = new FoodBatchRequest
            {
                FoodName = "Milk",
                SerialNumber = "1234567890",
                AnalysisTypeId = 1
            };

            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(validRequest, new ValidationContext(validRequest), validationResults, true);

            isValid.Should().BeTrue();
            validationResults.Should().BeEmpty();
        }
    }
}