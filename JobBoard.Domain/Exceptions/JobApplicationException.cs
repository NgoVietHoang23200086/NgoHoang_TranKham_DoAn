// ==========================================================
// CUSTOM EXCEPTION: JobApplicationException
// 
// === GIẢI THÍCH XỬ LÝ NGOẠI LỆ (Exception Handling) ===
// Custom Exception cho phép tạo các loại lỗi CỤ THỂ cho ứng dụng.
// Kế thừa từ Exception (lớp ngoại lệ cơ sở của .NET).
// 
// Lợi ích:
// 1. Phân biệt lỗi nghiệp vụ và lỗi hệ thống
// 2. Chứa thông tin bổ sung (ErrorCode) để client xử lý
// 3. Có thể catch riêng từng loại exception
// ==========================================================
namespace JobBoard.Domain.Exceptions
{
    public class JobApplicationException : Exception
    {
        /// <summary>
        /// Mã lỗi - giúp client xác định loại lỗi cụ thể
        /// Ví dụ: "DUPLICATE_APPLICATION", "JOB_EXPIRED", "INVALID_OPERATION"
        /// </summary>
        public string ErrorCode { get; }

        /// <summary>Constructor 1: Chỉ có message</summary>
        public JobApplicationException(string message)
            : base(message)
        {
            ErrorCode = "JOB_APP_ERROR";
        }

        /// <summary>Constructor 2: Có message và error code</summary>
        public JobApplicationException(string message, string errorCode)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Constructor 3: Có message, error code và inner exception
        /// Inner Exception: lưu exception gốc gây ra lỗi (exception chaining)
        /// </summary>
        public JobApplicationException(string message, string errorCode, Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }
    }
}
