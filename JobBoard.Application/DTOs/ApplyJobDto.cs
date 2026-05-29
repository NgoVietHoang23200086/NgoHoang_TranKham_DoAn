namespace JobBoard.Application.DTOs
{
    public class ApplyJobDto
    {
        public int CandidateId { get; set; }
        public int JobPostId { get; set; }
        public string? CoverLetter { get; set; }
    }
}
