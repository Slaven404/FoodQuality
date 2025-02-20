using System.ComponentModel.DataAnnotations;

namespace QualityManager.Models.Codes
{
    public class BaseCode
    {
        public long Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;
    }
}