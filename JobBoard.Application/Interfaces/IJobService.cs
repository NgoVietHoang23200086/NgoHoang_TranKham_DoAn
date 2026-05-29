using JobBoard.Application.DTOs;
namespace JobBoard.Application.Interfaces
{
    public interface IJobService
    {
        Task<IEnumerable<JobPostDto>> GetAllActiveJobsAsync();
        Task<JobPostDto?> GetJobByIdAsync(int id);
        Task<IEnumerable<JobPostDto>> SearchJobsAsync(JobSearchRequestDto searchRequest);
        Task<JobPostDto> CreateJobAsync(JobPostDto jobDto);
        Task<IEnumerable<JobPostDto>> GetJobsByEmployerAsync(int employerId);
        Task<IEnumerable<JobSearchResultDto>> GetRecommendedJobsAsync(int candidateId);
    }
}
