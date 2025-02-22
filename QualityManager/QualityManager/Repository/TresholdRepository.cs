using Microsoft.EntityFrameworkCore;
using QualityManager.Data;
using QualityManager.Models;

namespace QualityManager.Repository
{
    public class TresholdRepository : Repository<Treshold>, ITresholdRepository
    {
        public TresholdRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Treshold?> GetTresholdByAnalysisTypeIdAsync(long analysisTypeId)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.AnalysisTypeId == analysisTypeId);
        }
    }
}