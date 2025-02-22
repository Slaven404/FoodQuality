using Microsoft.AspNetCore.Mvc;
using QualityManager.DTOs.Requests;
using QualityManager.DTOs.Responses;
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

        [HttpPost("process")]
        public async Task<IActionResult> ProcessFood(FoodBatchRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            FoodBatchDetailsResponse? result = await _foodAnalysisService.CreateFoodAnalysisAsync(request);

            return Ok(result);
        }

        [HttpGet("status/{serial_number}")]
        public async Task<IActionResult> GetProcessStatus(string serial_number)
        {
            FoodProcessStatusDetailsResponse? result = await _foodAnalysisService.GetFoodAnalysisBySerialNumberAsync(serial_number);
            return Ok(result);
        }
    }
}