using QualityManager.DTOs.Requests;
using QualityManager.DTOs.Responses;
using QualityManager.Models;

namespace QualityManager.Services
{
    public interface IFoodAnalysisService
    {
        Task<FoodAnalysisResponse?> CreateFoodAnalysisAsync(FoodAnalysisRequest analysis);

        Task<FoodAnalysis?> GetFoodAnalysisBySerialNumberAsync(string serialNumber);
    }
}