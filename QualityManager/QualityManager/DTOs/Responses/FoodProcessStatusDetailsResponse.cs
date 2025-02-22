namespace QualityManager.DTOs.Responses
{
    public class FoodProcessStatusDetailsResponse
    {
        public long Id { get; set; }
        public string FoodName { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string AnalysisType { get; set; } = string.Empty;
        public string ProcessStatus { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
        public string AnalysisResult { get; set; } = string.Empty;
    }
}