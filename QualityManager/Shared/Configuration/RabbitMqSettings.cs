namespace Shared.Configuration
{
    public class RabbitMqSettings
    {
        public string HostName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FoodAnalysisRequestQueue { get; set; } = string.Empty;
        public string FoodAnalysisResponseQueue { get; set; } = string.Empty;
    }
}