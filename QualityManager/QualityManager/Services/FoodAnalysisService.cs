using AutoMapper;
using QualityManager.DTOs.Requests;
using QualityManager.DTOs.Responses;
using QualityManager.Exceptions;
using QualityManager.Models;
using QualityManager.Models.Codes;
using QualityManager.Repository;
using QualityManager.Resources;
using Shared.Enums;

namespace QualityManager.Services
{
    public class FoodAnalysisService : IFoodAnalysisService
    {
        private readonly IFoodAnalysisRepository _foodAnalysisRepository;
        private readonly ITresholdRepository _tresholdRepository;
        private readonly IMapper _mapper;
        private readonly AnalysisListener _foodAnalysisListener;

        public FoodAnalysisService(IFoodAnalysisRepository foodAnalysisRepository, IMapper mapper, AnalysisListener foodAnalysisListener, ITresholdRepository tresholdRepository)
        {
            _foodAnalysisRepository = foodAnalysisRepository;
            _mapper = mapper;
            _foodAnalysisListener = foodAnalysisListener;
            _tresholdRepository = tresholdRepository;
        }

        public async Task<FoodBatchDetailsResponse?> CreateFoodAnalysisAsync(FoodBatchRequest analysisRequest)
        {
            FoodAnalysis analysis = _mapper.Map<FoodAnalysis>(analysisRequest);
            analysis.ProcessStatusId = ProcessStatus.Pending.Id;

            await _foodAnalysisRepository.AddAsync(analysis);
            await _foodAnalysisRepository.SaveChangesAsync();

            await _foodAnalysisListener.SendFoodBatchToRabbitMqAsync(analysisRequest, CancellationToken.None);

            analysis = await _foodAnalysisRepository.GetDetailsByIdAsync(analysis.Id)
                             ?? throw new NotFoundException(string.Format(Translations.Exception_NotFound, nameof(FoodAnalysis), analysis.Id));
            return _mapper.Map<FoodBatchDetailsResponse>(analysis);
        }

        public async Task UpdateFoodProcessStatus(FoodAnalysisProcessResponse response)
        {
            FoodAnalysis foodAnalysis = await _foodAnalysisRepository.FindBySerialNumberAsync(response.SerialNumber)
                                              ?? throw new NotFoundException(string.Format(Translations.Exception_NotFound, nameof(FoodAnalysis), response.SerialNumber));
            if (string.IsNullOrEmpty(response.Result))
            {
                foodAnalysis.ProcessStatusId = ProcessStatus.Processing.Id;
            }
            else
            {
                foodAnalysis.ProcessStatusId = ProcessStatus.Completed.Id;
            }
            foodAnalysis.Result = response.Result;

            _foodAnalysisRepository.Update(foodAnalysis);

            await _foodAnalysisRepository.SaveChangesAsync();
        }

        public async Task<FoodProcessStatusDetailsResponse?> GetFoodAnalysisBySerialNumberAsync(string serialNumber)
        {
            FoodAnalysis foodAnalysis = await _foodAnalysisRepository.FindBySerialNumberAsync(serialNumber)
                                              ?? throw new NotFoundException(string.Format(Translations.Exception_NotFound, nameof(FoodAnalysis), serialNumber));

            FoodProcessStatusDetailsResponse details = _mapper.Map<FoodProcessStatusDetailsResponse>(foodAnalysis);
            details.AnalysisResult = await GenerateAnalysisResult(foodAnalysis.Result, foodAnalysis.AnalysisTypeId, foodAnalysis.ProcessStatusId);
            return details;
        }

        private async Task<string> GenerateAnalysisResult(string? result, long analysisTypeId, long processStatusId)
        {
            Treshold treshold = await _tresholdRepository.GetTresholdByAnalysisTypeIdAsync(analysisTypeId)
                                      ?? throw new NotFoundException(string.Format(Translations.Exception_NotFound, nameof(Treshold), analysisTypeId)); ;

            string processStatusText = "";

            switch (processStatusId)
            {
                case long _ when processStatusId == ProcessStatus.Pending.Id:
                    processStatusText = Translations.Peding;
                    break;

                case long _ when processStatusId == ProcessStatus.Processing.Id:
                    processStatusText = Translations.Processing;
                    break;

                case long _ when processStatusId == ProcessStatus.Completed.Id:
                    processStatusText = Translations.Completed;
                    break;

                default:
                    processStatusText = Translations.Unknown;
                    break;
            }

            string analysisResultText = "";

            if (processStatusId == ProcessStatus.Completed.Id)
            {
                if (long.TryParse(result, out long resultValue))
                {
                    if (resultValue > treshold.Low && resultValue < treshold.High)
                    {
                        analysisResultText = GetAnalysisMessageInRange((AnalysisTypeEnum)analysisTypeId);
                    }
                    else
                    {
                        analysisResultText = GetAnalysisMessageOutRange((AnalysisTypeEnum)analysisTypeId);
                    }
                }
            }
            return $"{processStatusText} {analysisResultText}";
        }

        private string GetAnalysisMessageInRange(AnalysisTypeEnum analysisType)
        {
            return analysisType switch
            {
                AnalysisTypeEnum.Microbiological => Translations.Microbiological_inrange,
                AnalysisTypeEnum.Chemical => Translations.Chemical_inrange,
                AnalysisTypeEnum.Sensory => Translations.Sensory_inrange,
                _ => Translations.Unknown,
            };
        }

        private string GetAnalysisMessageOutRange(AnalysisTypeEnum analysisType)
        {
            return analysisType switch
            {
                AnalysisTypeEnum.Microbiological => Translations.Microbiological_outrange,
                AnalysisTypeEnum.Chemical => Translations.Chemical_outrange,
                AnalysisTypeEnum.Sensory => Translations.Sensory_outrange,
                _ => Translations.Unknown,
            };
        }
    }
}