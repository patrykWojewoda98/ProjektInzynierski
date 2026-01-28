using Microsoft.EntityFrameworkCore;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Infrastructure.Context;

namespace ProjektIznynierski.Infrastructure.Repositories
{
    internal class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(ProjektInzynierskiDbContext dbContext) : base(dbContext)
        {
        }
        public async Task<List<Comment>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Comments
                .Include(c => c.Client)
                .ToListAsync(cancellationToken);
        }
        public async Task<IEnumerable<Comment>> GetByClientIdAsync(int clientId)
        {
            return await _dbContext.Comments
                .Where(c => c.ClientID == clientId).ToListAsync();
        }

        public async Task<IEnumerable<Comment>> GetByInvestInstrumentIdAsync(int investInstrumentId)
        {
            return await _dbContext.Comments
                .Where(c => c.InvestInstrumentID == investInstrumentId).ToListAsync();
        }
    }
}
