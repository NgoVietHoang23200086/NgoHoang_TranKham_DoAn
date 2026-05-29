// ==========================================================
// ENUM JobStatus: Trạng thái của tin tuyển dụng
// ==========================================================
namespace JobBoard.Domain.Enums
{
    public enum JobStatus
    {
        Pending = 0,    // Chờ duyệt
        Active = 1,     // Đang hoạt động
        Expired = 2,    // Đã hết hạn
        Closed = 3      // Đã đóng
    }
}
