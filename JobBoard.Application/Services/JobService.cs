using JobBoard.Application.DTOs;
using JobBoard.Application.Interfaces;
using JobBoard.Domain.Entities;
using JobBoard.Domain.Interfaces;
using JobBoard.Domain.Enums;

namespace JobBoard.Application.Services
{
    // SERVICE JobService: Xử lý logic nghiệp vụ liên quan đến tin tuyển dụng
    // Implement Interface IJobService (Dependency Inversion Principle)
    public class JobService : IJobService
    {
        private readonly IJobPostRepository _jobPostRepository;
        private readonly IUserRepository _userRepository;

        // Constructor Injection: Nhận dependency từ DI container
        public JobService(IJobPostRepository jobPostRepository, IUserRepository userRepository)
        {
            _jobPostRepository = jobPostRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<JobPostDto>> GetAllActiveJobsAsync()
        {
            var jobs = await _jobPostRepository.GetActiveJobsAsync();
            return jobs.Select(MapToDto);
        }

        public async Task<JobPostDto?> GetJobByIdAsync(int id)
        {
            var job = await _jobPostRepository.GetByIdAsync(id);
            return job != null ? MapToDto(job) : null;
        }

        // === THUẬT TOÁN TÌM KIẾM VÀ LỌC VIỆC LÀM ===
        // MÃ GIẢ (Pseudocode):
        // FUNCTION SearchJobs(keyword, minSalary, location):
        //   1. Bắt đầu với tất cả tin đang Active
        //   2. NẾU có keyword → Lọc theo Title HOẶC Description
        //   3. NẾU có minSalary → Lọc Salary >= minSalary
        //   4. NẾU có location → Lọc theo Location
        //   5. Sắp xếp theo ngày đăng mới nhất
        //   6. Trả về danh sách kết quả
        public async Task<IEnumerable<JobPostDto>> SearchJobsAsync(JobSearchRequestDto searchRequest)
        {
            var jobs = await _jobPostRepository.SearchAsync(
                searchRequest.Keyword, searchRequest.MinSalary, searchRequest.Location);
            return jobs.Select(MapToDto);
        }

        public async Task<JobPostDto> CreateJobAsync(JobPostDto jobDto)
        {
            var jobPost = new JobPost
            {
                Title = jobDto.Title,
                Description = jobDto.Description,
                Salary = jobDto.Salary,
                Location = jobDto.Location,
                RequiredSkills = jobDto.RequiredSkills,
                JobType = jobDto.JobType,
                EmployerId = jobDto.EmployerId,
                Status = JobStatus.Active,
                CreatedAt = DateTime.Now,
                ExpiryDate = jobDto.ExpiryDate
            };
            await _jobPostRepository.AddAsync(jobPost);
            jobDto.Id = jobPost.Id;
            return jobDto;
        }

        public async Task<IEnumerable<JobPostDto>> GetJobsByEmployerAsync(int employerId)
        {
            var jobs = await _jobPostRepository.GetJobsByEmployerAsync(employerId);
            return jobs.Select(MapToDto);
        }

        public async Task<IEnumerable<JobSearchResultDto>> GetRecommendedJobsAsync(int candidateId)
        {
            var candidate = await _userRepository.GetCandidateByIdAsync(candidateId);
            if (candidate == null) return Enumerable.Empty<JobSearchResultDto>();
            var allActiveJobs = await _jobPostRepository.GetActiveJobsAsync();
            var engine = new Algorithms.JobRecommendationEngine();
            return engine.GetRecommendations(candidate, allActiveJobs.ToList());
        }

        private static JobPostDto MapToDto(JobPost job)
        {
            return new JobPostDto
            {
                Id = job.Id, Title = job.Title, Description = job.Description,
                Salary = job.Salary, Location = job.Location,
                RequiredSkills = job.RequiredSkills, JobType = job.JobType,
                ExpiryDate = job.ExpiryDate,
                CompanyName = job.Employer?.CompanyName,
                CompanyLogoUrl = job.Employer?.LogoUrl,
                EmployerId = job.EmployerId
            };
        }
    }
}
