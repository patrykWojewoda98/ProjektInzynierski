using Microsoft.EntityFrameworkCore;
using ProjektIznynierski.Domain.Abstractions;
using ProjektIznynierski.Domain.Entities;
using ProjektIznynierski.Infrastructure.Context;

namespace ProjektIznynierski.Infrastructure.Repositories
{
    internal class GenericRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ProjektInzynierskiDbContext _dbContext;
        public GenericRepository(ProjektInzynierskiDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<T>()
                .SingleOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public void Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
        }
        public void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
        }
        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }
    }
}
