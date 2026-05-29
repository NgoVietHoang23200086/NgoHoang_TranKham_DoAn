using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using JobBoard.Application.DTOs;
using JobBoard.Application.Interfaces;

namespace JobBoard.Web.Pages.Jobs
{
    public class IndexModel : PageModel
    {
        private readonly IJobService _jobService;
        public IEnumerable<JobPostDto>? Jobs { get; set; }

        [BindProperty(SupportsGet = true)] public string? Keyword { get; set; }
        [BindProperty(SupportsGet = true)] public decimal? MinSalary { get; set; }
        [BindProperty(SupportsGet = true)] public string? Location { get; set; }

        public IndexModel(IJobService jobService) { _jobService = jobService; }

        public async Task OnGetAsync()
        {
            if (!string.IsNullOrWhiteSpace(Keyword) || MinSalary.HasValue || !string.IsNullOrWhiteSpace(Location))
            {
                Jobs = await _jobService.SearchJobsAsync(new JobSearchRequestDto
                {
                    Keyword = Keyword, MinSalary = MinSalary, Location = Location
                });
            }
            else
            {
                Jobs = await _jobService.GetAllActiveJobsAsync();
            }
        }
    }
}
