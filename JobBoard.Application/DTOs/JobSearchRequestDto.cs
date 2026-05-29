namespace JobBoard.Application.DTOs
{
    public class JobSearchRequestDto
    {
        public string? Keyword { get; set; }
        public decimal? MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }
        public string? Location { get; set; }
        public string? JobType { get; set; }
    }
}
