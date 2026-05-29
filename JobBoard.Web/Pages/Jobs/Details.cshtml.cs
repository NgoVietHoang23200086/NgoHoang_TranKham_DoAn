using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using JobBoard.Application.DTOs;
using JobBoard.Application.Interfaces;
using JobBoard.Domain.Exceptions;

namespace JobBoard.Web.Pages.Jobs
{
    public class DetailsModel : PageModel
    {
        private readonly IJobService _jobService;
        private readonly IApplicationService _applicationService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public JobPostDto? Job { get; set; }
        public string? Message { get; set; }
        public bool IsError { get; set; }

        public DetailsModel(IJobService jobService, IApplicationService applicationService, IWebHostEnvironment webHostEnvironment)
        {
            _jobService = jobService;
            _applicationService = applicationService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Job = await _jobService.GetJobByIdAsync(id);
            if (Job == null) return NotFound();
            return Page();
        }

        // === MINH HỌA try-catch VỚI CUSTOM EXCEPTION & XỬ LÝ FILE UPLOAD ===
        public async Task<IActionResult> OnPostAsync(int jobId, IFormFile? cvFile)
        {
            Job = await _jobService.GetJobByIdAsync(jobId);
            if (Job == null) return NotFound();

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToPage("/Account/Login");

            if (cvFile == null || cvFile.Length == 0)
            {
                Message = "❌ Vui lòng chọn file CV trước khi ứng tuyển.";
                IsError = true;
                return Page();
            }

            try
            {
                // Kiểm tra và tạo thư mục uploads trong wwwroot
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Kiểm tra định dạng file an toàn
                var extension = Path.GetExtension(cvFile.FileName).ToLower();
                var allowedExtensions = new[] { ".pdf", ".doc", ".docx", ".png", ".jpg", ".jpeg" };
                if (!allowedExtensions.Contains(extension))
                {
                    Message = "❌ Định dạng file không hợp lệ! Vui lòng tải file PDF, Word (.doc, .docx) hoặc ảnh.";
                    IsError = true;
                    return Page();
                }

                // Đặt tên file duy nhất tránh xung đột ghi đè
                var uniqueFileName = $"cv_{userId.Value}_{jobId}_{DateTime.Now.Ticks}{extension}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Lưu file vật lý lên server
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await cvFile.CopyToAsync(fileStream);
                }

                // Đường dẫn tương đối lưu vào trường CoverLetter trong database
                var relativePath = $"/uploads/{uniqueFileName}";

                await _applicationService.ApplyToJobAsync(new ApplyJobDto
                {
                    CandidateId = userId.Value,
                    JobPostId = jobId,
                    CoverLetter = relativePath
                });

                Message = "✅ Ứng tuyển thành công! CV của bạn đã được tải lên và gửi tới nhà tuyển dụng.";
                IsError = false;
            }
            catch (DuplicateApplicationException ex)
            {
                Message = $"⚠️ {ex.Message}";
                IsError = true;
            }
            catch (ExpiredJobPostException ex)
            {
                Message = $"⚠️ {ex.Message}";
                IsError = true;
            }
            catch (JobApplicationException ex)
            {
                Message = $"❌ Lỗi: {ex.Message} (Mã: {ex.ErrorCode})";
                IsError = true;
            }
            catch (Exception ex)
            {
                Message = $"❌ Đã xảy ra lỗi hệ thống: {ex.Message}";
                IsError = true;
            }

            return Page();
        }
    }
}
