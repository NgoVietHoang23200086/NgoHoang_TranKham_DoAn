namespace JobBoard.Application.DTOs
{
    public class JobSearchResultDto
    {
        public JobPostDto Job { get; set; } = new();
        public double MatchScore { get; set; }
        public string MatchReason { get; set; } = string.Empty;
    }
}
