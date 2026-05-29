// ==========================================================
// Custom Exception: Ứng viên ứng tuyển trùng lặp
// Kế thừa từ JobApplicationException → Exception Hierarchy
// 
// Ví dụ sử dụng trong try-catch:
// try { applyToJob(); }
// catch (DuplicateApplicationException ex) { /* Xử lý trùng lặp */ }
// catch (JobApplicationException ex) { /* Xử lý lỗi chung */ }
// ==========================================================
namespace JobBoard.Domain.Exceptions
{
    public class DuplicateApplicationException : JobApplicationException
    {
        public int CandidateId { get; }
        public int JobPostId { get; }

        public DuplicateApplicationException(int candidateId, int jobPostId)
            : base($"Ứng viên (ID: {candidateId}) đã ứng tuyển vào tin tuyển dụng (ID: {jobPostId}) trước đó.",
                   "DUPLICATE_APPLICATION")
        {
            CandidateId = candidateId;
            JobPostId = jobPostId;
        }
    }
}
