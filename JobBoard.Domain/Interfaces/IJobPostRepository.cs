using JobBoard.Domain.Entities;

namespace JobBoard.Domain.Interfaces
{
    /// <summary>
    /// Interface IJobPostRepository: Kế thừa IRepository và bổ sung phương thức
    /// đặc thù cho JobPost (tìm kiếm, lọc, gợi ý)
    /// </summary>
    public interface IJobPostRepository : IRepository<JobPost>
    {
        Task<IEnumerable<JobPost>> SearchAsync(string? keyword, decimal? minSalary, string? location);
        Task<IEnumerable<JobPost>> GetActiveJobsAsync();
        Task<IEnumerable<JobPost>> GetJobsByEmployerAsync(int employerId);
        Task<IEnumerable<JobPost>> GetRecommendedJobsAsync(List<string> candidateSkills, decimal? expectedSalary, string? preferredLocation);
    }
}
