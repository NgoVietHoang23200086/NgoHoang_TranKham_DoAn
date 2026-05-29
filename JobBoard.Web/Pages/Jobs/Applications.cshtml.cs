using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using JobBoard.Application.DTOs;
using JobBoard.Application.Interfaces;
using JobBoard.Domain.Entities;
using JobBoard.Domain.Enums;

namespace JobBoard.Web.Pages.Jobs
{
    public class ApplicationsModel : PageModel
    {
        private readonly IJobService _jobService;
        private readonly IApplicationService _applicationService;
        private readonly IAuthService _authService;

        public JobPostDto? Job { get; set; }
        public IEnumerable<JobApplication> Applications { get; set; } = new List<JobApplication>();

        [TempData]
        public string? SuccessMessage { get; set; }

        [TempData]
        public string? ErrorMessage { get; set; }

        public ApplicationsModel(IJobService jobService, IApplicationService applicationService, IAuthService authService)
        {
            _jobService = jobService;
            _applicationService = applicationService;
            _authService = authService;
        }

        public async Task<IActionResult> OnGetAsync(int jobId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToPage("/Account/Login");

            var user = await _authService.GetCurrentUserAsync(userId.Value);
            if (user == null || user.Role != UserRole.Employer)
            {
                return RedirectToPage("/Dashboard/Index");
            }

            Job = await _jobService.GetJobByIdAsync(jobId);
            if (Job == null || Job.EmployerId != userId.Value)
            {
                ErrorMessage = "Không tìm thấy tin tuyển dụng này hoặc bạn không có quyền xem.";
                return RedirectToPage("/Dashboard/Index");
            }

            Applications = await _applicationService.GetApplicationsByJobAsync(jobId);
            return Page();
        }

        public async Task<IActionResult> OnPostAcceptAsync(int applicationId, int jobId, string? notes)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToPage("/Account/Login");

            var user = await _authService.GetCurrentUserAsync(userId.Value);
            if (user == null || user.Role != UserRole.Employer)
            {
                return RedirectToPage("/Dashboard/Index");
            }

            var job = await _jobService.GetJobByIdAsync(jobId);
            if (job == null || job.EmployerId != userId.Value)
            {
                ErrorMessage = "Thao tác không hợp lệ.";
                return RedirectToPage("/Dashboard/Index");
            }

            try
            {
                await _applicationService.AcceptApplicationAsync(applicationId, notes);
                SuccessMessage = "Đã phê duyệt đơn ứng tuyển thành công.";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Lỗi khi duyệt đơn: {ex.Message}";
            }

            return RedirectToPage(new { jobId });
        }

        public async Task<IActionResult> OnPostRejectAsync(int applicationId, int jobId, string? notes)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToPage("/Account/Login");

            var user = await _authService.GetCurrentUserAsync(userId.Value);
            if (user == null || user.Role != UserRole.Employer)
            {
                return RedirectToPage("/Dashboard/Index");
            }

            var job = await _jobService.GetJobByIdAsync(jobId);
            if (job == null || job.EmployerId != userId.Value)
            {
                ErrorMessage = "Thao tác không hợp lệ.";
                return RedirectToPage("/Dashboard/Index");
            }

            try
            {
                await _applicationService.RejectApplicationAsync(applicationId, notes);
                SuccessMessage = "Đã từ chối đơn ứng tuyển.";
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Lỗi khi từ chối đơn: {ex.Message}";
            }

            return RedirectToPage(new { jobId });
        }
    }
}
