using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using JobBoard.Application.DTOs;
using JobBoard.Application.Interfaces;
using JobBoard.Domain.Entities;
using JobBoard.Domain.Enums;

namespace JobBoard.Web.Pages.Dashboard
{
    public class IndexModel : PageModel
    {
        private readonly IJobService _jobService;
        private readonly IApplicationService _applicationService;
        private readonly IAuthService _authService;

        public bool IsCandidate { get; set; }
        public bool IsEmployer { get; set; }
        public string? DashboardText { get; set; }
        public int ApplicationCount { get; set; }
        public int RecommendedCount { get; set; }
        public Candidate? CandidateProfile { get; set; }
        public IEnumerable<JobSearchResultDto>? RecommendedJobs { get; set; }
        public IEnumerable<JobApplication>? Applications { get; set; }
        public IEnumerable<JobPostDto>? EmployerJobs { get; set; }
        public int TotalApplicationsReceived { get; set; }
        public Dictionary<int, int> JobApplicationCounts { get; set; } = new();
        public List<JobApplication> RecentFeedbacks { get; set; } = new();

        public IndexModel(IJobService jobService, IApplicationService applicationService, IAuthService authService)
        {
            _jobService = jobService;
            _applicationService = applicationService;
            _authService = authService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToPage("/Account/Login");

            var user = await _authService.GetCurrentUserAsync(userId.Value);
            if (user == null) return RedirectToPage("/Account/Login");

            // === TÍNH ĐA HÌNH: Gọi DisplayDashboard() ===
            // Cùng một phương thức, nhưng kết quả trả về khác nhau tùy theo loại đối tượng
            DashboardText = user.DisplayDashboard();

            if (user is Candidate candidate)
            {
                IsCandidate = true;
                CandidateProfile = candidate;
                Applications = await _applicationService.GetApplicationsByCandidateAsync(userId.Value);
                ApplicationCount = Applications.Count();

                // Lấy danh sách phản hồi mới (đã được Duyệt hoặc Từ chối)
                if (Applications != null)
                {
                    RecentFeedbacks = Applications.Where(a => a.Status != ApplicationStatus.Pending).ToList();
                }
                
                // Gọi thuật toán gợi ý việc làm
                RecommendedJobs = await _jobService.GetRecommendedJobsAsync(userId.Value);
                RecommendedCount = RecommendedJobs.Count();
            }
            else if (user is Employer employer)
            {
                IsEmployer = true;
                EmployerJobs = await _jobService.GetJobsByEmployerAsync(userId.Value);
                
                TotalApplicationsReceived = 0;
                JobApplicationCounts = new Dictionary<int, int>();

                if (EmployerJobs != null)
                {
                    foreach (var job in EmployerJobs)
                    {
                        var apps = await _applicationService.GetApplicationsByJobAsync(job.Id);
                        var count = apps.Count();
                        JobApplicationCounts[job.Id] = count;
                        TotalApplicationsReceived += count;
                    }
                }
            }

            return Page();
        }
    }
}
