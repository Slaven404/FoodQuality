using QualityManager.Resources;
using System.ComponentModel.DataAnnotations;

namespace QualityManager.DTOs.Requests
{
    public class FoodBatchRequest
    {
        [Required(ErrorMessage = nameof(Translations.Validation_FoodNameRequired))]
        public string FoodName { get; set; } = string.Empty;

        [Required(ErrorMessage = nameof(Translations.Validation_SerialNumberRequired))]
        [MaxLength(32, ErrorMessage = nameof(Translations.Validation_SerialNumberTooLong))]
        public string SerialNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = nameof(Translations.Validation_AnalysisTypeRequired))]
        [Range(1, long.MaxValue, ErrorMessage = nameof(Translations.Validation_AnalysisTypeMaxValue))]
        public long AnalysisTypeId { get; set; }
    }
}