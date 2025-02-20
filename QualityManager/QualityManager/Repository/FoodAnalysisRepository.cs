using Microsoft.EntityFrameworkCore;
using QualityManager.Data;
using QualityManager.Models;

namespace QualityManager.Repository
{
    public class FoodAnalysisRepository : Repository<FoodAnalysis>, IFoodAnalysisRepository
    {
        public FoodAnalysisRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<FoodAnalysis?> FindBySerialNumberAsync(string serialNumber)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.SerialNumber == serialNumber);
        }

        public async Task<FoodAnalysis?> GetDetailsByIdAsync(long id)
        {
            return await _dbSet.Include(x => x.AnalysisType).Include(x => x.ProcessStatus).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}