namespace QualityManager.Models.Codes
{
    public class ProcessStatus : BaseCode
    {
        public static readonly ProcessStatus Pending = new ProcessStatus { Id = 1, Name = "Pending" };
        public static readonly ProcessStatus Processing = new ProcessStatus { Id = 2, Name = "Processing" };
        public static readonly ProcessStatus Completed = new ProcessStatus { Id = 3, Name = "Completed" };
    }
}