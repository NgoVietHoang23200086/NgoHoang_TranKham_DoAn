using Microsoft.EntityFrameworkCore;
using JobBoard.Domain.Interfaces;
using JobBoard.Infrastructure.Data;

namespace JobBoard.Infrastructure.Repositories
{
    // GENERIC REPOSITORY: Base CRUD operations
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);
        public virtual async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();
        public virtual async Task AddAsync(T entity) { await _dbSet.AddAsync(entity); await _context.SaveChangesAsync(); }
        public virtual async Task UpdateAsync(T entity) { _dbSet.Update(entity); await _context.SaveChangesAsync(); }
        public virtual async Task DeleteAsync(int id) { var e = await _dbSet.FindAsync(id); if (e != null) { _dbSet.Remove(e); await _context.SaveChangesAsync(); } }
        public virtual async Task<bool> ExistsAsync(int id) => await _dbSet.FindAsync(id) != null;
    }
}
