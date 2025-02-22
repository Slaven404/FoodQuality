namespace QualityManager.DTOs.Responses
{
    public class FoodBatchDetailsResponse
    {
        public long Id { get; set; }
        public string FoodName { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public long AnalysisTypeId { get; set; }
        public string AnalysisType { get; set; } = string.Empty;
        public long ProcessStatusId { get; set; }
        public string ProcessStatus { get; set; } = string.Empty;
    }
}