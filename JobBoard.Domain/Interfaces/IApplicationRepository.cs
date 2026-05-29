using JobBoard.Domain.Entities;

namespace JobBoard.Domain.Interfaces
{
    public interface IApplicationRepository : IRepository<JobApplication>
    {
        Task<IEnumerable<JobApplication>> GetByCandidateIdAsync(int candidateId);
        Task<IEnumerable<JobApplication>> GetByJobPostIdAsync(int jobPostId);
        Task<bool> HasAlreadyAppliedAsync(int candidateId, int jobPostId);
    }
}
