using JobBoard.Domain.Enums;

// ==========================================================
// CLASS Employer: Lớp Nhà tuyển dụng - Kế thừa từ User
// 
// === TÍNH KẾ THỪA (Inheritance) ===
// Employer kế thừa từ User: Id, Name, Email, Password, CreatedAt, Role...
// 
// === TÍNH ĐA HÌNH (Polymorphism) ===
// Override DisplayDashboard() với giao diện KHÁC với Candidate:
// - Candidate: Hiển thị việc đã ứng tuyển, gợi ý việc làm
// - Employer: Hiển thị tin đã đăng, đơn ứng tuyển nhận được
// → Cùng phương thức, KHÁC kết quả → Đa hình!
// ==========================================================
namespace JobBoard.Domain.Entities
{
    public class Employer : User
    {
        public Employer()
        {
            Role = UserRole.Employer;
        }

        // ==========================================
        // THUỘC TÍNH RIÊNG CỦA EMPLOYER
        // ==========================================

        /// <summary>Tên công ty</summary>
        public string CompanyName { get; set; } = string.Empty;

        /// <summary>Mô tả về công ty</summary>
        public string? CompanyDescription { get; set; }

        /// <summary>Địa chỉ công ty</summary>
        public string? CompanyAddress { get; set; }

        /// <summary>Website công ty</summary>
        public string? CompanyWebsite { get; set; }

        /// <summary>Logo công ty (URL)</summary>
        public string? LogoUrl { get; set; }

        // Navigation Property - Quan hệ 1-N với JobPost
        public virtual ICollection<JobPost> JobPosts { get; set; } = new List<JobPost>();

        // ==========================================
        // OVERRIDE ABSTRACT METHODS - TÍNH ĐA HÌNH
        // ==========================================

        /// <summary>
        /// Override DisplayDashboard() - TÍNH ĐA HÌNH.
        /// Employer hiển thị: Tin đã đăng, Số đơn ứng tuyển, Quản lý tuyển dụng.
        /// (Khác hoàn toàn với Candidate.DisplayDashboard())
        /// </summary>
        public override string DisplayDashboard()
        {
            return $"=== BẢNG ĐIỀU KHIỂN NHÀ TUYỂN DỤNG ===\n" +
                   $"Xin chào, {Name} - {CompanyName}!\n" +
                   $"🏢 Công ty: {CompanyName}\n" +
                   $"📝 Số tin tuyển dụng: {JobPosts.Count}\n" +
                   $"🌐 Website: {CompanyWebsite ?? "Chưa cập nhật"}\n" +
                   $"📊 Quản lý đơn ứng tuyển và tuyển dụng";
        }

        /// <summary>
        /// Override SendNotification() - ĐA HÌNH.
        /// Nhà tuyển dụng nhận thông báo về: đơn ứng tuyển mới, tin sắp hết hạn.
        /// </summary>
        public override string SendNotification(string message)
        {
            return $"📧 [Thông báo cho NTD {CompanyName} ({Name})]: {message}";
        }

        public override string GetProfileSummary()
        {
            return base.GetProfileSummary() + $" | Công ty: {CompanyName}";
        }

        // ==========================================
        // BUSINESS METHODS
        // ==========================================

        /// <summary>
        /// Tạo tin tuyển dụng mới - Minh họa phương thức nghiệp vụ trong Entity.
        /// </summary>
        public JobPost CreateJobPost(string title, string description, decimal salary,
                                     string location, string requiredSkills, int validDays = 30)
        {
            return new JobPost
            {
                Title = title,
                Description = description,
                Salary = salary,
                Location = location,
                RequiredSkills = requiredSkills,
                EmployerId = this.Id,
                Employer = this,
                Status = JobStatus.Active,
                CreatedAt = DateTime.Now,
                ExpiryDate = DateTime.Now.AddDays(validDays)
            };
        }
    }
}
