using QualityManager.Resources;
using System.ComponentModel.DataAnnotations;

namespace QualityManager.DTOs.Requests
{
    public class FoodBatchRequest
    {
        [Required(ErrorMessage = nameof(ValidationMessages.FoodNameRequired))]
        public string FoodName { get; set; } = string.Empty;

        [Required(ErrorMessage = nameof(ValidationMessages.SerialNumberRequired))]
        [MaxLength(32, ErrorMessage = nameof(ValidationMessages.SerialNumberTooLong))]
        public string SerialNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = nameof(ValidationMessages.AnalysisTypeRequired))]
        [Range(1, long.MaxValue, ErrorMessage = nameof(ValidationMessages.AnalysisTypeMaxValue))]
        public long AnalysisTypeId { get; set; }
    }
}