using Microsoft.AspNetCore.Mvc;
using QualityManager.DTOs.Requests;
using QualityManager.DTOs.Responses;
using QualityManager.Models;
using QualityManager.Services;

namespace QualityManager.Controllers
{
    public class FoodController : BaseController
    {
        private readonly IFoodAnalysisService _foodAnalysisService;

        public FoodController(IFoodAnalysisService foodAnalysisService)
        {
            _foodAnalysisService = foodAnalysisService;
        }

        [HttpPost("/process")]
        public async Task<IActionResult> ProcessFood(FoodAnalysisRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            FoodAnalysisResponse? result = await _foodAnalysisService.CreateFoodAnalysisAsync(request);

            return Ok(result);
        }
    }
}