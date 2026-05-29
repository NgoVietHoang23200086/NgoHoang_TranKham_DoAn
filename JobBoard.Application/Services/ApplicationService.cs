using JobBoard.Application.DTOs;
using JobBoard.Application.Interfaces;
using JobBoard.Domain.Entities;
using JobBoard.Domain.Enums;
using JobBoard.Domain.Exceptions;
using JobBoard.Domain.Interfaces;

namespace JobBoard.Application.Services
{
    // SERVICE ApplicationService: Quản lý đơn ứng tuyển
    // === MINH HỌA XỬ LÝ NGOẠI LỆ (Exception Handling) ===
    // try-catch-finally với Custom Exceptions
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IJobPostRepository _jobPostRepository;
        private readonly IUserRepository _userRepository;

        public ApplicationService(
            IApplicationRepository applicationRepository,
            IJobPostRepository jobPostRepository,
            IUserRepository userRepository)
        {
            _applicationRepository = applicationRepository;
            _jobPostRepository = jobPostRepository;
            _userRepository = userRepository;
        }

        // === ỨNG TUYỂN VIỆC LÀM - Minh họa Exception Handling đầy đủ ===
        // MÃ GIẢ (Pseudocode):
        // FUNCTION ApplyToJob(candidateId, jobPostId, coverLetter):
        //   TRY:
        //     1. Kiểm tra ứng viên tồn tại → THROW nếu không
        //     2. Kiểm tra tin tuyển dụng tồn tại → THROW nếu không
        //     3. Kiểm tra tin đã hết hạn → THROW ExpiredJobPostException
        //     4. Kiểm tra ứng tuyển trùng lặp → THROW DuplicateApplicationException
        //     5. Tạo đơn ứng tuyển mới
        //     6. Lưu vào database
        //   CATCH DuplicateApplicationException: → Ném lại
        //   CATCH ExpiredJobPostException: → Ném lại
        //   CATCH Exception: → Bọc trong JobApplicationException
        //   FINALLY: → Ghi log hoàn tất
        public async Task<JobApplication> ApplyToJobAsync(ApplyJobDto applyDto)
        {
            bool isSuccess = false;
            try
            {
                // Bước 1: Kiểm tra ứng viên tồn tại
                var candidate = await _userRepository.GetCandidateByIdAsync(applyDto.CandidateId);
                if (candidate == null)
                    throw new JobApplicationException(
                        $"Không tìm thấy ứng viên với ID: {applyDto.CandidateId}", "CANDIDATE_NOT_FOUND");

                // Bước 2: Kiểm tra tin tuyển dụng tồn tại
                var jobPost = await _jobPostRepository.GetByIdAsync(applyDto.JobPostId);
                if (jobPost == null)
                    throw new JobApplicationException(
                        $"Không tìm thấy tin tuyển dụng với ID: {applyDto.JobPostId}", "JOB_NOT_FOUND");

                // Bước 3: Kiểm tra tin đã hết hạn → Custom Exception
                if (jobPost.IsExpired())
                    throw new ExpiredJobPostException(jobPost.Id, jobPost.Title);

                // Bước 4: Kiểm tra ứng tuyển trùng lặp → Custom Exception
                bool alreadyApplied = await _applicationRepository
                    .HasAlreadyAppliedAsync(applyDto.CandidateId, applyDto.JobPostId);
                if (alreadyApplied)
                    throw new DuplicateApplicationException(applyDto.CandidateId, applyDto.JobPostId);

                // Bước 5: Tạo đơn ứng tuyển
                var application = new JobApplication
                {
                    CandidateId = applyDto.CandidateId,
                    JobPostId = applyDto.JobPostId,
                    CoverLetter = applyDto.CoverLetter,
                    Status = ApplicationStatus.Pending,
                    AppliedAt = DateTime.Now
                };

                // Bước 6: Lưu vào database
                await _applicationRepository.AddAsync(application);
                isSuccess = true;
                return application;
            }
            catch (DuplicateApplicationException) { throw; }
            catch (ExpiredJobPostException) { throw; }
            catch (JobApplicationException) { throw; }
            catch (Exception ex)
            {
                // Bọc exception hệ thống trong JobApplicationException
                throw new JobApplicationException(
                    $"Lỗi hệ thống khi xử lý đơn ứng tuyển: {ex.Message}",
                    "SYSTEM_ERROR", ex);
            }
            finally
            {
                // FINALLY: Luôn thực thi dù có exception hay không
                var status = isSuccess ? "THÀNH CÔNG" : "THẤT BẠI";
                Console.WriteLine($"[LOG] Xử lý ứng tuyển - Candidate: {applyDto.CandidateId}, " +
                                  $"Job: {applyDto.JobPostId} - Kết quả: {status}");
            }
        }

        public async Task<IEnumerable<JobApplication>> GetApplicationsByCandidateAsync(int candidateId)
            => await _applicationRepository.GetByCandidateIdAsync(candidateId);

        public async Task<IEnumerable<JobApplication>> GetApplicationsByJobAsync(int jobPostId)
            => await _applicationRepository.GetByJobPostIdAsync(jobPostId);

        public async Task AcceptApplicationAsync(int applicationId, string? notes)
        {
            var app = await _applicationRepository.GetByIdAsync(applicationId)
                ?? throw new JobApplicationException($"Không tìm thấy đơn ứng tuyển ID: {applicationId}");
            app.Accept(notes);
            await _applicationRepository.UpdateAsync(app);
        }

        public async Task RejectApplicationAsync(int applicationId, string? notes)
        {
            var app = await _applicationRepository.GetByIdAsync(applicationId)
                ?? throw new JobApplicationException($"Không tìm thấy đơn ứng tuyển ID: {applicationId}");
            app.Reject(notes);
            await _applicationRepository.UpdateAsync(app);
        }
    }
}
