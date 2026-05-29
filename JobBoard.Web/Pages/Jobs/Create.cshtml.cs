using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using JobBoard.Application.DTOs;
using JobBoard.Application.Interfaces;

namespace JobBoard.Web.Pages.Jobs
{
    public class CreateModel : PageModel
    {
        private readonly IJobService _jobService;
        [BindProperty] public JobPostDto JobDto { get; set; } = new() { ExpiryDate = DateTime.Now.AddDays(30) };
        public string? Message { get; set; }
        public bool IsError { get; set; }

        public CreateModel(IJobService jobService) { _jobService = jobService; }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("UserRole") != "Employer")
                return RedirectToPage("/Account/Login");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToPage("/Account/Login");
            
            JobDto.EmployerId = userId.Value;
            await _jobService.CreateJobAsync(JobDto);
            Message = "✅ Đăng tin tuyển dụng thành công!";
            return Page();
        }
    }
}
