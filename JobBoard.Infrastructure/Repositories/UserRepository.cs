using Microsoft.EntityFrameworkCore;
using JobBoard.Domain.Entities;
using JobBoard.Domain.Interfaces;
using JobBoard.Infrastructure.Data;

namespace JobBoard.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context) { }

        public async Task<User?> GetByEmailAsync(string email)
            => await _dbSet.FirstOrDefaultAsync(u => u.Email == email);

        public async Task<Candidate?> GetCandidateByIdAsync(int id)
            => await _context.Candidates.Include(c => c.Applications)
                .ThenInclude(a => a.JobPost).ThenInclude(j => j!.Employer)
                .FirstOrDefaultAsync(c => c.Id == id);

        public async Task<Employer?> GetEmployerByIdAsync(int id)
            => await _context.Employers.Include(e => e.JobPosts)
                .ThenInclude(j => j.Applications)
                .FirstOrDefaultAsync(e => e.Id == id);

        public async Task<bool> EmailExistsAsync(string email)
            => await _dbSet.AnyAsync(u => u.Email == email);
    }
}
