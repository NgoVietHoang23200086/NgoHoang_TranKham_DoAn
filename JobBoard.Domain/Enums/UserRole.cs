// ==========================================================
// ENUM UserRole: Định nghĩa vai trò người dùng trong hệ thống
// Sử dụng Enum thay vì "magic string" để đảm bảo Type Safety
// ==========================================================
namespace JobBoard.Domain.Enums
{
    public enum UserRole
    {
        Candidate = 0,  // Ứng viên tìm việc
        Employer = 1    // Nhà tuyển dụng
    }
}
