using JobBoard.Domain.Entities;

// ==========================================================
// INTERFACE IJobSeeker: Định nghĩa hợp đồng (contract) cho đối tượng tìm việc
// 
// === GIẢI THÍCH OOP ===
// Interface là một "bản thiết kế" (blueprint) định nghĩa CÁC HÀNH VI 
// mà một class phải thực hiện.
// Interface chỉ chứa khai báo phương thức (method signature), 
// KHÔNG chứa phần thân (implementation).
// 
// Lợi ích của Interface:
// 1. Tách biệt "CÁI GÌ" (what) khỏi "LÀM THẾ NÀO" (how) → Loose Coupling
// 2. Một class có thể implement NHIỀU interface → Đa kế thừa hành vi
// 3. Dễ dàng mock/test → Dependency Injection
// 4. Tuân thủ nguyên tắc SOLID (Interface Segregation Principle)
// ==========================================================
namespace JobBoard.Domain.Interfaces
{
    public interface IJobSeeker
    {
        /// <summary>Tìm kiếm việc làm theo từ khóa, mức lương, địa điểm</summary>
        List<JobPost> SearchJobs(string keyword, decimal? minSalary, string? location);

        /// <summary>Ứng tuyển vào một tin tuyển dụng</summary>
        void ApplyToJob(JobPost job);

        /// <summary>Lấy danh sách việc làm gợi ý dựa trên kỹ năng</summary>
        List<JobPost> GetRecommendedJobs();
    }
}
