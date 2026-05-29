using Microsoft.EntityFrameworkCore;
using JobBoard.Domain.Entities;
using JobBoard.Domain.Interfaces;
using JobBoard.Infrastructure.Data;

namespace JobBoard.Infrastructure.Repositories
{
    public class ApplicationRepository : GenericRepository<JobApplication>, IApplicationRepository
    {
        public ApplicationRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<JobApplication?> GetByIdAsync(int id)
            => await _dbSet.Include(a => a.Candidate).Include(a => a.JobPost)
                .ThenInclude(j => j!.Employer).FirstOrDefaultAsync(a => a.Id == id);

        public async Task<IEnumerable<JobApplication>> GetByCandidateIdAsync(int candidateId)
            => await _dbSet.Include(a => a.JobPost).ThenInclude(j => j!.Employer)
                .Where(a => a.CandidateId == candidateId)
                .OrderByDescending(a => a.AppliedAt).ToListAsync();

        public async Task<IEnumerable<JobApplication>> GetByJobPostIdAsync(int jobPostId)
            => await _dbSet.Include(a => a.Candidate)
                .Where(a => a.JobPostId == jobPostId)
                .OrderByDescending(a => a.AppliedAt).ToListAsync();

        public async Task<bool> HasAlreadyAppliedAsync(int candidateId, int jobPostId)
            => await _dbSet.AnyAsync(a => a.CandidateId == candidateId && a.JobPostId == jobPostId);
    }
}
