namespace JobBoard.Application.DTOs
{
    public class JobPostDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public string Location { get; set; } = string.Empty;
        public string RequiredSkills { get; set; } = string.Empty;
        public string? JobType { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanyLogoUrl { get; set; }
        public int EmployerId { get; set; }
    }
}
