// ==========================================================
// ENUM ApplicationStatus: Trạng thái đơn ứng tuyển
// ==========================================================
namespace JobBoard.Domain.Enums
{
    public enum ApplicationStatus
    {
        Pending = 0,    // Đang chờ xử lý
        Reviewed = 1,   // Đã xem
        Accepted = 2,   // Đã chấp nhận
        Rejected = 3    // Đã từ chối
    }
}
