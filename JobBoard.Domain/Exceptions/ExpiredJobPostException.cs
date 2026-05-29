// ==========================================================
// Custom Exception: Tin tuyển dụng đã hết hạn
// Được throw khi ứng viên cố ứng tuyển vào tin đã hết hạn
// ==========================================================
namespace JobBoard.Domain.Exceptions
{
    public class ExpiredJobPostException : JobApplicationException
    {
        public int JobPostId { get; }
        public string JobTitle { get; }

        public ExpiredJobPostException(int jobPostId, string jobTitle)
            : base($"Tin tuyển dụng '{jobTitle}' (ID: {jobPostId}) đã hết hạn. Không thể ứng tuyển.",
                   "JOB_EXPIRED")
        {
            JobPostId = jobPostId;
            JobTitle = jobTitle;
        }
    }
}
