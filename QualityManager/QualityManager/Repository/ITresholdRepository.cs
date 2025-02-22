using QualityManager.Models;

namespace QualityManager.Repository
{
    public interface ITresholdRepository : IRepository<Treshold>
    {
        Task<Treshold?> GetTresholdByAnalysisTypeIdAsync(long analysisTypeId);
    }
}