using JobBoard.Domain.Enums;

// ==========================================================
// ABSTRACT CLASS User: Lớp trừu tượng đại diện cho người dùng hệ thống
// 
// === GIẢI THÍCH OOP - TÍNH TRỪU TƯỢNG (Abstraction) ===
// Abstract Class là lớp KHÔNG THỂ tạo đối tượng trực tiếp (không thể new User()).
// Nó đóng vai trò là "khuôn mẫu" (template) cho các lớp con.
// 
// === GIẢI THÍCH OOP - TÍNH ĐÓNG GÓI (Encapsulation) ===
// Encapsulation ẩn dữ liệu nội bộ và kiểm soát truy cập thông qua Properties.
// Ví dụ: trường _passwordHash là private, chỉ có thể truy cập qua Property Password.
// 
// === GIẢI THÍCH OOP - TÍNH KẾ THỪA (Inheritance) ===
// Candidate và Employer sẽ KẾ THỪA từ User, nhận được tất cả thuộc tính chung.
// 
// === GIẢI THÍCH OOP - TÍNH ĐA HÌNH (Polymorphism) ===
// Phương thức abstract DisplayDashboard() và SendNotification() sẽ được override
// bởi từng lớp con với cách xử lý KHÁC NHAU.
// ==========================================================
namespace JobBoard.Domain.Entities
{
    public abstract class User
    {
        // ==========================================
        // AUTO-IMPLEMENTED PROPERTIES
        // Compiler tự tạo backing field ẩn phía sau
        // ==========================================

        /// <summary>Mã định danh duy nhất của người dùng (Primary Key)</summary>
        public int Id { get; set; }

        /// <summary>Họ và tên người dùng</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Địa chỉ email (dùng để đăng nhập)</summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>Số điện thoại liên hệ</summary>
        public string? Phone { get; set; }

        /// <summary>Ngày tạo tài khoản</summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// Vai trò người dùng (Candidate hoặc Employer)
        /// 'protected set' chỉ cho phép lớp con thay đổi giá trị
        /// </summary>
        public UserRole Role { get; protected set; }

        // ==========================================
        // BACKING FIELD + ENCAPSULATION
        // Minh họa kỹ thuật đóng gói dữ liệu
        // ==========================================

        /// <summary>
        /// Backing Field: Trường dữ liệu riêng tư lưu mật khẩu đã mã hóa.
        /// Không thể truy cập trực tiếp từ bên ngoài class.
        /// </summary>
        private string _passwordHash = string.Empty;

        /// <summary>
        /// Property Password với logic đóng gói:
        /// - GET: Trả về mật khẩu đã mã hóa
        /// - SET: Tự động mã hóa trước khi lưu (Encapsulation)
        /// </summary>
        public string Password
        {
            get => _passwordHash;
            set => _passwordHash = HashPassword(value);
        }

        // ==========================================
        // ABSTRACT METHODS - Buộc lớp con PHẢI override
        // Thể hiện TÍNH ĐA HÌNH (Polymorphism)
        // ==========================================

        /// <summary>
        /// Phương thức trừu tượng: Hiển thị bảng điều khiển (Dashboard).
        /// Candidate và Employer sẽ có dashboard KHÁC NHAU → TÍNH ĐA HÌNH
        /// </summary>
        public abstract string DisplayDashboard();

        /// <summary>
        /// Phương thức trừu tượng: Gửi thông báo đến người dùng.
        /// Candidate nhận thông báo về việc làm mới phù hợp.
        /// Employer nhận thông báo về đơn ứng tuyển mới.
        /// → Cùng một phương thức, nhưng xử lý KHÁC NHAU ở mỗi lớp con.
        /// </summary>
        public abstract string SendNotification(string message);

        // ==========================================
        // VIRTUAL METHOD - Lớp con CÓ THỂ ghi đè
        // ==========================================

        /// <summary>
        /// Phương thức ảo: Lấy tóm tắt hồ sơ người dùng.
        /// Virtual cho phép lớp con GHI ĐÈ (override) nếu muốn.
        /// </summary>
        public virtual string GetProfileSummary()
        {
            return $"{Name} ({Email}) - Vai trò: {Role}";
        }

        // ==========================================
        // PRIVATE METHOD - Chỉ dùng nội bộ trong class
        // ==========================================

        /// <summary>
        /// Phương thức private: Mã hóa mật khẩu.
        /// Đây là minh họa Encapsulation - logic xử lý ẩn bên trong class.
        /// Trong thực tế sử dụng BCrypt hoặc PBKDF2.
        /// </summary>
        private string HashPassword(string plainPassword)
        {
            if (string.IsNullOrEmpty(plainPassword))
                return string.Empty;

            // Sử dụng SHA256 đơn giản cho mục đích minh họa
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var bytes = System.Text.Encoding.UTF8.GetBytes(plainPassword + "_JobBoard_Salt");
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        /// <summary>
        /// Phương thức public: Xác thực mật khẩu.
        /// So sánh mật khẩu nhập vào với mật khẩu đã mã hóa.
        /// </summary>
        public bool VerifyPassword(string plainPassword)
        {
            var hashedInput = HashPassword(plainPassword);
            return _passwordHash == hashedInput;
        }
    }
}
