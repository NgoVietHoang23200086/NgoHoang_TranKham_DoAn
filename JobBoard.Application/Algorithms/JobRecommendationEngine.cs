using JobBoard.Application.DTOs;
using JobBoard.Domain.Entities;

namespace JobBoard.Application.Algorithms
{
    // ==========================================================
    // THUẬT TOÁN GỢI Ý VIỆC LÀM PHÙ HỢP CHO ỨNG VIÊN
    // 
    // === MÃ GIẢ (PSEUDOCODE) ===
    // ALGORITHM JobRecommendation:
    //   INPUT:  candidate (ứng viên), allJobs (tất cả tin active)
    //   OUTPUT: recommendedJobs (sắp xếp theo điểm giảm dần)
    //   
    //   1. Khởi tạo scoredJobs rỗng
    //   2. Lấy candidateSkills từ ứng viên
    //   3. VỚI MỖI job TRONG allJobs:
    //      a. NẾU job hết hạn → BỎ QUA
    //      b. score = 0
    //      c. Tính điểm KỸ NĂNG (50%): matchedSkills/totalSkills * 50
    //      d. Tính điểm LƯƠNG (30%): nếu đáp ứng = 30, ngược lại = tỷ lệ * 30
    //      e. Tính điểm ĐỊA ĐIỂM (20%): trùng = 20, khác = 0
    //      f. Thêm (job, score) vào scoredJobs
    //   4. Sắp xếp theo score giảm dần
    //   5. Trả về TOP 10
    // ==========================================================
    public class JobRecommendationEngine
    {
        private const double SKILL_WEIGHT = 50.0;
        private const double SALARY_WEIGHT = 30.0;
        private const double LOCATION_WEIGHT = 20.0;
        private const int MAX_RECOMMENDATIONS = 10;

        public List<JobSearchResultDto> GetRecommendations(Candidate candidate, List<JobPost> allJobs)
        {
            var scoredJobs = new List<JobSearchResultDto>();
            var candidateSkills = candidate.GetSkillsList()
                .Select(s => s.ToLower().Trim()).ToList();

            foreach (var job in allJobs)
            {
                if (job.IsExpired()) continue;

                double score = 0;
                var reasons = new List<string>();

                // Tính điểm Kỹ năng (50%)
                double skillScore = CalculateSkillScore(candidateSkills, job.GetRequiredSkillsList());
                score += skillScore;
                if (skillScore > 0) reasons.Add($"Kỹ năng: {skillScore:F1}/{SKILL_WEIGHT}");

                // Tính điểm Lương (30%)
                double salaryScore = CalculateSalaryScore(candidate.ExpectedSalary, job.Salary);
                score += salaryScore;
                if (salaryScore > 0) reasons.Add($"Lương: {salaryScore:F1}/{SALARY_WEIGHT}");

                // Tính điểm Địa điểm (20%)
                double locationScore = CalculateLocationScore(candidate.PreferredLocation, job.Location);
                score += locationScore;
                if (locationScore > 0) reasons.Add($"Địa điểm: {locationScore:F1}/{LOCATION_WEIGHT}");

                if (score > 0)
                {
                    scoredJobs.Add(new JobSearchResultDto
                    {
                        Job = new JobPostDto
                        {
                            Id = job.Id, Title = job.Title, Description = job.Description,
                            Salary = job.Salary, Location = job.Location,
                            RequiredSkills = job.RequiredSkills, JobType = job.JobType,
                            ExpiryDate = job.ExpiryDate,
                            CompanyName = job.Employer?.CompanyName,
                            CompanyLogoUrl = job.Employer?.LogoUrl,
                            EmployerId = job.EmployerId
                        },
                        MatchScore = Math.Round(score, 1),
                        MatchReason = string.Join(" | ", reasons)
                    });
                }
            }

            return scoredJobs.OrderByDescending(j => j.MatchScore)
                .Take(MAX_RECOMMENDATIONS).ToList();
        }

        private double CalculateSkillScore(List<string> candidateSkills, List<string> jobSkills)
        {
            if (!jobSkills.Any() || !candidateSkills.Any()) return 0;
            var jobSkillsLower = jobSkills.Select(s => s.ToLower().Trim()).ToList();
            int matchedCount = candidateSkills
                .Count(cs => jobSkillsLower.Any(js => js.Contains(cs) || cs.Contains(js)));
            return (matchedCount / (double)jobSkillsLower.Count) * SKILL_WEIGHT;
        }

        private double CalculateSalaryScore(decimal expectedSalary, decimal jobSalary)
        {
            if (expectedSalary <= 0) return SALARY_WEIGHT;
            if (jobSalary >= expectedSalary) return SALARY_WEIGHT;
            return (double)(jobSalary / expectedSalary) * SALARY_WEIGHT;
        }

        private double CalculateLocationScore(string? preferredLocation, string jobLocation)
        {
            if (string.IsNullOrWhiteSpace(preferredLocation)) return LOCATION_WEIGHT;
            if (jobLocation.Contains(preferredLocation, StringComparison.OrdinalIgnoreCase))
                return LOCATION_WEIGHT;
            return 0;
        }
    }
}
