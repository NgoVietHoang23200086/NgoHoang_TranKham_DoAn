using JobBoard.Domain.Enums;

// ==========================================================
// CLASS JobApplication: Đơn ứng tuyển
// Đại diện cho mối quan hệ NHIỀU-NHIỀU giữa Candidate và JobPost
// Minh họa: Composition (Đơn ứng tuyển PHẢI thuộc về 1 Candidate + 1 JobPost)
// ==========================================================
namespace JobBoard.Domain.Entities
{
    public class JobApplication
    {
        public int Id { get; set; }
        public string? CoverLetter { get; set; }
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;
        public DateTime AppliedAt { get; set; } = DateTime.Now;
        public string? EmployerNotes { get; set; }

        // FOREIGN KEYS & NAVIGATION PROPERTIES
        public int CandidateId { get; set; }
        public virtual Candidate? Candidate { get; set; }
        public int JobPostId { get; set; }
        public virtual JobPost? JobPost { get; set; }

        // BUSINESS METHODS
        public void Accept(string? notes = null)
        {
            Status = ApplicationStatus.Accepted;
            EmployerNotes = notes;
        }

        public void Reject(string? notes = null)
        {
            Status = ApplicationStatus.Rejected;
            EmployerNotes = notes;
        }

        public void MarkAsReviewed()
        {
            if (Status == ApplicationStatus.Pending)
                Status = ApplicationStatus.Reviewed;
        }
    }
}
