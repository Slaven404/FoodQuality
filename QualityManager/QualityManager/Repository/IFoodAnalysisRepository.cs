using QualityManager.Models;

namespace QualityManager.Repository
{
    public interface IFoodAnalysisRepository : IRepository<FoodAnalysis>
    {
        Task<FoodAnalysis?> FindBySerialNumberAsync(string serialNumber);

        Task<FoodAnalysis?> GetDetailsByIdAsync(long id);
    }
}