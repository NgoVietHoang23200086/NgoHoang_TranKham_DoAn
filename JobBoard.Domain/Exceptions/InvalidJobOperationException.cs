// ==========================================================
// Custom Exception: Thao tác không hợp lệ với tin tuyển dụng
// Ví dụ: Duyệt tin đã active, đóng tin đã đóng
// ==========================================================
namespace JobBoard.Domain.Exceptions
{
    public class InvalidJobOperationException : JobApplicationException
    {
        public InvalidJobOperationException(string message)
            : base(message, "INVALID_OPERATION")
        {
        }
    }
}
