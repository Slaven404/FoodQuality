using AutoMapper;
using QualityManager.DTOs.Requests;
using QualityManager.DTOs.Responses;
using QualityManager.Models;
using QualityManager.Models.Codes;
using QualityManager.Repository;

namespace QualityManager.Services
{
    public class FoodAnalysisService : IFoodAnalysisService
    {
        private readonly IFoodAnalysisRepository _foodAnalysisRepository;
        private readonly IMapper _mapper;

        public FoodAnalysisService(IFoodAnalysisRepository foodAnalysisRepository, IMapper mapper)
        {
            _foodAnalysisRepository = foodAnalysisRepository;
            _mapper = mapper;
        }

        public async Task<FoodAnalysisResponse?> CreateFoodAnalysisAsync(FoodAnalysisRequest analysisRequest)
        {
            FoodAnalysis analysis = _mapper.Map<FoodAnalysis>(analysisRequest);
            analysis.ProcessStatusId = ProcessStatus.Pending.Id;

            await _foodAnalysisRepository.AddAsync(analysis);
            await _foodAnalysisRepository.SaveChangesAsync();

            analysis = await _foodAnalysisRepository.GetDetailsByIdAsync(analysis.Id);
            return _mapper.Map<FoodAnalysisResponse>(analysis);
        }

        public async Task<FoodAnalysis?> GetFoodAnalysisBySerialNumberAsync(string serialNumber)
        {
            return await _foodAnalysisRepository.FindBySerialNumberAsync(serialNumber);
        }
    }
}