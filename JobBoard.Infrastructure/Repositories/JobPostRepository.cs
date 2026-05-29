using Microsoft.EntityFrameworkCore;
using JobBoard.Domain.Entities;
using JobBoard.Domain.Enums;
using JobBoard.Domain.Interfaces;
using JobBoard.Infrastructure.Data;

namespace JobBoard.Infrastructure.Repositories
{
    public class JobPostRepository : GenericRepository<JobPost>, IJobPostRepository
    {
        public JobPostRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<JobPost?> GetByIdAsync(int id)
        {
            return await _dbSet.Include(j => j.Employer)
                .Include(j => j.Applications).ThenInclude(a => a.Candidate)
                .FirstOrDefaultAsync(j => j.Id == id);
        }

        public async Task<IEnumerable<JobPost>> GetActiveJobsAsync()
        {
            return await _dbSet.Include(j => j.Employer)
                .Where(j => j.Status == JobStatus.Active && j.ExpiryDate > DateTime.Now)
                .OrderByDescending(j => j.CreatedAt).ToListAsync();
        }

        // Tìm kiếm việc làm theo từ khóa, lương, địa điểm (LINQ)
        public async Task<IEnumerable<JobPost>> SearchAsync(string? keyword, decimal? minSalary, string? location)
        {
            var query = _dbSet.Include(j => j.Employer)
                .Where(j => j.Status == JobStatus.Active && j.ExpiryDate > DateTime.Now).AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.ToLower();
                query = query.Where(j => j.Title.ToLower().Contains(keyword) ||
                    j.Description.ToLower().Contains(keyword) || j.RequiredSkills.ToLower().Contains(keyword));
            }
            if (minSalary.HasValue && minSalary.Value > 0)
                query = query.Where(j => j.Salary >= minSalary.Value);
            if (!string.IsNullOrWhiteSpace(location))
                query = query.Where(j => j.Location.ToLower().Contains(location.ToLower()));

            return await query.OrderByDescending(j => j.CreatedAt).ToListAsync();
        }

        public async Task<IEnumerable<JobPost>> GetJobsByEmployerAsync(int employerId)
        {
            return await _dbSet.Include(j => j.Applications)
                .Where(j => j.EmployerId == employerId)
                .OrderByDescending(j => j.CreatedAt).ToListAsync();
        }

        public async Task<IEnumerable<JobPost>> GetRecommendedJobsAsync(
            List<string> candidateSkills, decimal? expectedSalary, string? preferredLocation)
            => await GetActiveJobsAsync();
    }
}
