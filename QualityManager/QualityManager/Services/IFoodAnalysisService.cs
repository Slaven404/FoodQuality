using QualityManager.DTOs.Requests;
using QualityManager.DTOs.Responses;

namespace QualityManager.Services
{
    public interface IFoodAnalysisService
    {
        Task<FoodBatchDetailsResponse?> CreateFoodAnalysisAsync(FoodBatchRequest analysis);

        Task<FoodProcessStatusDetailsResponse?> GetFoodAnalysisBySerialNumberAsync(string serialNumber);

        Task UpdateFoodProcessStatus(FoodAnalysisProcessResponse response);
    }
}