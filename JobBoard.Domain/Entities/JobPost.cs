using JobBoard.Domain.Enums;
using JobBoard.Domain.Exceptions;

// ==========================================================
// CLASS JobPost: Tin tuyển dụng
// 
// Minh họa: Encapsulation, Auto-implemented Properties, Business Methods
// Class này chứa cả DỮ LIỆU (Properties) và HÀNH VI (Methods)
// → Đây là nguyên tắc cốt lõi của OOP
// ==========================================================
namespace JobBoard.Domain.Entities
{
    public class JobPost
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public string Location { get; set; } = string.Empty;

        /// <summary>
        /// Kỹ năng yêu cầu (lưu dạng chuỗi phân tách bằng dấu phẩy).
        /// Ví dụ: "C#, ASP.NET Core, Entity Framework, SQL Server"
        /// </summary>
        public string RequiredSkills { get; set; } = string.Empty;

        public JobStatus Status { get; set; } = JobStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime ExpiryDate { get; set; }
        public string? JobType { get; set; }

        // ==========================================
        // FOREIGN KEY & NAVIGATION PROPERTIES
        // ==========================================

        public int EmployerId { get; set; }
        public virtual Employer? Employer { get; set; }
        public virtual ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();

        // ==========================================
        // BUSINESS METHODS (Phương thức nghiệp vụ)
        // ==========================================

        /// <summary>Kiểm tra tin tuyển dụng đã hết hạn chưa</summary>
        public bool IsExpired()
        {
            return DateTime.Now > ExpiryDate || Status == JobStatus.Expired;
        }

        /// <summary>
        /// Duyệt tin tuyển dụng - Sử dụng Custom Exception khi tin không hợp lệ.
        /// </summary>
        public void Approve()
        {
            if (Status != JobStatus.Pending)
                throw new InvalidJobOperationException(
                    $"Không thể duyệt tin '{Title}' vì trạng thái hiện tại là {Status}");
            Status = JobStatus.Active;
        }

        /// <summary>Đóng tin tuyển dụng</summary>
        public void Close()
        {
            if (Status == JobStatus.Closed)
                throw new InvalidJobOperationException(
                    $"Tin '{Title}' đã được đóng trước đó");
            Status = JobStatus.Closed;
        }

        /// <summary>Lấy danh sách kỹ năng yêu cầu dưới dạng List</summary>
        public List<string> GetRequiredSkillsList()
        {
            if (string.IsNullOrWhiteSpace(RequiredSkills))
                return new List<string>();
            return RequiredSkills.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                                .ToList();
        }

        /// <summary>Format mức lương hiển thị</summary>
        public string GetFormattedSalary()
        {
            return $"{Salary:N0} VNĐ/tháng";
        }
    }
}
