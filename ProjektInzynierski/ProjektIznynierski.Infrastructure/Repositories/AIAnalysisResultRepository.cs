using Microsoft.EntityFrameworkCore;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Infrastructure.Context;

namespace ProjektIznynierski.Infrastructure.Repositories
{
    internal class AIAnalysisResultRepository : GenericRepository<AIAnalysisResult>, IAIAnalysisResultRepository
    {
        public AIAnalysisResultRepository(ProjektInzynierskiDbContext dbContext) : base(dbContext)
        {

        }
        
        
        
    }
}
