using QualityManager.Models.Codes;
using System.ComponentModel.DataAnnotations;

namespace QualityManager.Models
{
    public class FoodAnalysis
    {
        public long Id { get; set; }

        [Required]
        public string FoodName { get; set; } = string.Empty;

        [Required, MaxLength(64)]
        public string SerialNumber { get; set; } = string.Empty;

        [Required]
        public long AnalysisTypeId { get; set; }

        public AnalysisType AnalysisType { get; set; } = null!;

        [Required]
        public long ProcessStatusId { get; set; }

        public ProcessStatus ProcessStatus { get; set; } = null!;
    }
}