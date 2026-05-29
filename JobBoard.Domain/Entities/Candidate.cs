using JobBoard.Domain.Enums;
using JobBoard.Domain.Interfaces;

// ==========================================================
// CLASS Candidate: Lớp Ứng viên - Kế thừa từ User và Implement IJobSeeker
// 
// === TÍNH KẾ THỪA (Inheritance) ===
// Candidate kế thừa TẤT CẢ thuộc tính và phương thức từ User:
// Id, Name, Email, Password, CreatedAt, Role, VerifyPassword()...
// → Tái sử dụng code, không cần viết lại.
// 
// === TÍNH ĐA HÌNH (Polymorphism) ===
// Override DisplayDashboard() và SendNotification() với logic riêng cho ứng viên.
// 
// === INTERFACE IMPLEMENTATION ===
// Implement IJobSeeker: SearchJobs(), ApplyToJob(), GetRecommendedJobs()
// → Một class có thể vừa kế thừa Abstract Class, vừa implement Interface.
// ==========================================================
namespace JobBoard.Domain.Entities
{
    public class Candidate : User, IJobSeeker
    {
        // ==========================================
        // CONSTRUCTOR - Thiết lập giá trị ban đầu
        // ==========================================

        /// <summary>
        /// Constructor: Tự động gán Role = Candidate khi tạo đối tượng.
        /// Sử dụng 'protected set' từ lớp cha để chỉ cho phép lớp con thay đổi Role.
        /// </summary>
        public Candidate()
        {
            Role = UserRole.Candidate;
        }

        // ==========================================
        // THUỘC TÍNH RIÊNG CỦA CANDIDATE
        // ==========================================

        /// <summary>Đường dẫn file CV/Resume</summary>
        public string? ResumeUrl { get; set; }

        /// <summary>
        /// Danh sách kỹ năng (lưu dạng chuỗi phân tách bằng dấu phẩy trong DB).
        /// Ví dụ: "C#, ASP.NET, SQL, JavaScript"
        /// </summary>
        public string? Skills { get; set; }

        /// <summary>Mức lương mong muốn (VNĐ/tháng)</summary>
        public decimal ExpectedSalary { get; set; }

        /// <summary>Địa điểm làm việc mong muốn</summary>
        public string? PreferredLocation { get; set; }

        /// <summary>Số năm kinh nghiệm</summary>
        public int YearsOfExperience { get; set; }

        // Navigation Property - Quan hệ 1-N với JobApplication
        /// <summary>Danh sách đơn ứng tuyển của ứng viên</summary>
        public virtual ICollection<JobApplication> Applications { get; set; } = new List<JobApplication>();

        // ==========================================
        // OVERRIDE ABSTRACT METHODS - TÍNH ĐA HÌNH
        // ==========================================

        /// <summary>
        /// Override DisplayDashboard() - TÍNH ĐA HÌNH.
        /// Candidate hiển thị: Việc làm đã ứng tuyển, Gợi ý việc làm, Trạng thái đơn.
        /// (Khác hoàn toàn với Employer.DisplayDashboard())
        /// </summary>
        public override string DisplayDashboard()
        {
            return $"=== BẢNG ĐIỀU KHIỂN ỨNG VIÊN ===\n" +
                   $"Xin chào, {Name}!\n" +
                   $"📋 Kỹ năng: {Skills ?? "Chưa cập nhật"}\n" +
                   $"💰 Mức lương mong muốn: {ExpectedSalary:N0} VNĐ\n" +
                   $"📍 Địa điểm: {PreferredLocation ?? "Chưa cập nhật"}\n" +
                   $"📄 Số đơn đã ứng tuyển: {Applications.Count}\n" +
                   $"🔔 Xem việc làm gợi ý phù hợp với kỹ năng của bạn";
        }

        /// <summary>
        /// Override SendNotification() - ĐA HÌNH.
        /// Ứng viên nhận thông báo về: việc làm mới phù hợp, trạng thái đơn ứng tuyển.
        /// </summary>
        public override string SendNotification(string message)
        {
            return $"📧 [Thông báo cho Ứng viên {Name}]: {message}";
        }

        /// <summary>
        /// Override GetProfileSummary() - GHI ĐÈ Virtual Method.
        /// Bổ sung thông tin kỹ năng và kinh nghiệm vào tóm tắt hồ sơ.
        /// </summary>
        public override string GetProfileSummary()
        {
            // Gọi phương thức của lớp cha bằng base.GetProfileSummary()
            return base.GetProfileSummary() +
                   $" | Kỹ năng: {Skills} | Kinh nghiệm: {YearsOfExperience} năm";
        }

        // ==========================================
        // IMPLEMENT INTERFACE IJobSeeker
        // ==========================================

        /// <summary>
        /// Implement IJobSeeker.SearchJobs() - Tìm kiếm việc làm.
        /// Logic thực tế sẽ ở Application Layer (Service), đây là placeholder.
        /// </summary>
        public List<JobPost> SearchJobs(string keyword, decimal? minSalary, string? location)
        {
            return new List<JobPost>();
        }

        /// <summary>
        /// Implement IJobSeeker.ApplyToJob() - Ứng tuyển việc làm.
        /// </summary>
        public void ApplyToJob(JobPost job)
        {
            if (job == null)
                throw new ArgumentNullException(nameof(job));
            if (job.IsExpired())
                throw new Exceptions.ExpiredJobPostException(job.Id, job.Title);
        }

        /// <summary>
        /// Implement IJobSeeker.GetRecommendedJobs() - Gợi ý việc làm.
        /// </summary>
        public List<JobPost> GetRecommendedJobs()
        {
            return new List<JobPost>();
        }

        // ==========================================
        // HELPER METHOD
        // ==========================================

        /// <summary>
        /// Lấy danh sách kỹ năng dưới dạng List (tách từ chuỗi phân tách bằng dấu phẩy).
        /// </summary>
        public List<string> GetSkillsList()
        {
            if (string.IsNullOrWhiteSpace(Skills))
                return new List<string>();

            return Skills.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                         .ToList();
        }
    }
}
