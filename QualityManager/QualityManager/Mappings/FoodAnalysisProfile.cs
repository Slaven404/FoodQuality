using AutoMapper;
using QualityManager.DTOs.Requests;
using QualityManager.DTOs.Responses;
using QualityManager.Models;

namespace QualityManager.Mappings
{
    public class FoodAnalysisProfile : Profile
    {
        public FoodAnalysisProfile()
        {
            CreateMap<FoodAnalysisRequest, FoodAnalysis>();

            CreateMap<FoodAnalysis, FoodAnalysisResponse>()
                .ForMember(dest => dest.AnalysisType, opt => opt.MapFrom(src => src.AnalysisType.Name))
                .ForMember(dest => dest.ProcessStatus, opt => opt.MapFrom(src => src.ProcessStatus.Name));
        }
    }
}