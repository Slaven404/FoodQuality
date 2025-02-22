using QualityManager.Models.Codes;
using System.ComponentModel.DataAnnotations;

namespace QualityManager.Models
{
    public class Treshold
    {
        public long Id { get; set; }
        public long Low { get; set; }
        public long High { get; set; }

        [Required]
        public long AnalysisTypeId { get; set; }

        public AnalysisType AnalysisType { get; set; } = null!;
    }
}