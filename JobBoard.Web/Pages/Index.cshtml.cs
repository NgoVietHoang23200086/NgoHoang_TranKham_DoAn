using Microsoft.AspNetCore.Mvc.RazorPages;
using JobBoard.Application.DTOs;
using JobBoard.Application.Interfaces;

namespace JobBoard.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IJobService _jobService;
        public IEnumerable<JobPostDto>? LatestJobs { get; set; }

        public IndexModel(IJobService jobService)
        {
            _jobService = jobService;
        }

        public async Task OnGetAsync()
        {
            var allJobs = await _jobService.GetAllActiveJobsAsync();
            LatestJobs = allJobs.Take(6);
        }
    }
}
