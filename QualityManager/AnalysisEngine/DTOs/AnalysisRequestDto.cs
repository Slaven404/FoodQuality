namespace AnalysisEngine.DTOs
{
    public class AnalysisRequestDto
    {
        public string FoodName { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public long AnalysisTypeId { get; set; }
    }
}