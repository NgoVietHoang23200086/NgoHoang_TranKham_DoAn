using JobBoard.Application.DTOs;
using JobBoard.Domain.Entities;
namespace JobBoard.Application.Interfaces
{
    public interface IApplicationService
    {
        Task<JobApplication> ApplyToJobAsync(ApplyJobDto applyDto);
        Task<IEnumerable<JobApplication>> GetApplicationsByCandidateAsync(int candidateId);
        Task<IEnumerable<JobApplication>> GetApplicationsByJobAsync(int jobPostId);
        Task AcceptApplicationAsync(int applicationId, string? notes);
        Task RejectApplicationAsync(int applicationId, string? notes);
    }
}
