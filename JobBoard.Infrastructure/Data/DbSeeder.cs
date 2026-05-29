using JobBoard.Domain.Entities;
using JobBoard.Domain.Enums;

namespace JobBoard.Infrastructure.Data
{
    // DbSeeder: Tạo dữ liệu mẫu cho ứng dụng
    public static class DbSeeder
    {
        public static void SeedData(ApplicationDbContext context)
        {
            if (context.Users.Any()) return;

            var employer1 = new Employer
            {
                Name = "Ngô Việt Hoàng", Email = "employer1@jobboard.vn",
                Phone = "0901234567", CompanyName = "FPT Software",
                CompanyDescription = "Công ty phần mềm hàng đầu Việt Nam",
                CompanyAddress = "Hà Nội", CompanyWebsite = "https://fpt-software.com",
                CreatedAt = DateTime.Now
            };
            employer1.Password = "123456";

            var employer2 = new Employer
            {
                Name = "Trần Đình Khâm", Email = "employer2@jobboard.vn",
                Phone = "0912345678", CompanyName = "VNG Corporation",
                CompanyDescription = "Công ty công nghệ giải trí",
                CompanyAddress = "TP. Hồ Chí Minh", CompanyWebsite = "https://vng.com.vn",
                CreatedAt = DateTime.Now
            };
            employer2.Password = "123456";

            var employer3 = new Employer
            {
                Name = "Lê Hoàng Dũng", Email = "employer3@jobboard.vn",
                Phone = "0923456789", CompanyName = "Viettel Solutions",
                CompanyDescription = "Giải pháp công nghệ Viettel",
                CompanyAddress = "Hà Nội", CreatedAt = DateTime.Now
            };
            employer3.Password = "123456";

            context.Employers.AddRange(employer1, employer2, employer3);
            context.SaveChanges();

            var candidate1 = new Candidate
            {
                Name = "Phạm Minh Tuấn", Email = "candidate1@jobboard.vn",
                Phone = "0934567890",
                Skills = "C#, ASP.NET Core, Entity Framework, SQL Server, JavaScript",
                ExpectedSalary = 20000000, PreferredLocation = "Hà Nội",
                YearsOfExperience = 3, CreatedAt = DateTime.Now
            };
            candidate1.Password = "123456";

            var candidate2 = new Candidate
            {
                Name = "Nguyễn Thu Hà", Email = "candidate2@jobboard.vn",
                Phone = "0945678901",
                Skills = "Java, Spring Boot, MySQL, Docker, Kubernetes",
                ExpectedSalary = 25000000, PreferredLocation = "TP. Hồ Chí Minh",
                YearsOfExperience = 5, CreatedAt = DateTime.Now
            };
            candidate2.Password = "123456";

            var candidate3 = new Candidate
            {
                Name = "Đỗ Quang Huy", Email = "candidate3@jobboard.vn",
                Phone = "0956789012",
                Skills = "Python, Django, React, Node.js, MongoDB",
                ExpectedSalary = 18000000, PreferredLocation = "Đà Nẵng",
                YearsOfExperience = 2, CreatedAt = DateTime.Now
            };
            candidate3.Password = "123456";

            context.Candidates.AddRange(candidate1, candidate2, candidate3);
            context.SaveChanges();

            var jobs = new List<JobPost>
            {
                new JobPost { Title = "Lập trình viên .NET (C#)", Description = "Phát triển ứng dụng web bằng ASP.NET Core. Yêu cầu kinh nghiệm Entity Framework, LINQ, REST API.", Salary = 22000000, Location = "Hà Nội", RequiredSkills = "C#, ASP.NET Core, Entity Framework, SQL Server", JobType = "Full-time", Status = JobStatus.Active, EmployerId = employer1.Id, CreatedAt = DateTime.Now, ExpiryDate = DateTime.Now.AddDays(30) },
                new JobPost { Title = "Frontend Developer (React)", Description = "Xây dựng giao diện web hiện đại với React.js, TypeScript.", Salary = 20000000, Location = "TP. Hồ Chí Minh", RequiredSkills = "React, JavaScript, TypeScript, CSS, HTML", JobType = "Full-time", Status = JobStatus.Active, EmployerId = employer2.Id, CreatedAt = DateTime.Now, ExpiryDate = DateTime.Now.AddDays(30) },
                new JobPost { Title = "Java Backend Developer", Description = "Phát triển microservices bằng Java Spring Boot.", Salary = 28000000, Location = "TP. Hồ Chí Minh", RequiredSkills = "Java, Spring Boot, MySQL, Docker, Kubernetes", JobType = "Full-time", Status = JobStatus.Active, EmployerId = employer2.Id, CreatedAt = DateTime.Now, ExpiryDate = DateTime.Now.AddDays(30) },
                new JobPost { Title = "Python Developer", Description = "Phát triển backend API bằng Python Django/Flask.", Salary = 18000000, Location = "Đà Nẵng", RequiredSkills = "Python, Django, Flask, PostgreSQL", JobType = "Full-time", Status = JobStatus.Active, EmployerId = employer3.Id, CreatedAt = DateTime.Now, ExpiryDate = DateTime.Now.AddDays(30) },
                new JobPost { Title = "DevOps Engineer", Description = "Quản lý hạ tầng cloud, CI/CD pipeline.", Salary = 35000000, Location = "Hà Nội", RequiredSkills = "Docker, Kubernetes, AWS, Terraform, CI/CD", JobType = "Full-time", Status = JobStatus.Active, EmployerId = employer1.Id, CreatedAt = DateTime.Now, ExpiryDate = DateTime.Now.AddDays(30) },
                new JobPost { Title = "Full-stack Developer (Node.js + React)", Description = "Phát triển cả frontend và backend cho sản phẩm SaaS.", Salary = 24000000, Location = "TP. Hồ Chí Minh", RequiredSkills = "Node.js, React, MongoDB, Express, TypeScript", JobType = "Full-time", Status = JobStatus.Active, EmployerId = employer2.Id, CreatedAt = DateTime.Now, ExpiryDate = DateTime.Now.AddDays(30) },
                new JobPost { Title = "Mobile Developer (Flutter)", Description = "Xây dựng ứng dụng di động cross-platform bằng Flutter/Dart.", Salary = 22000000, Location = "Hà Nội", RequiredSkills = "Flutter, Dart, Firebase, REST API, Git", JobType = "Full-time", Status = JobStatus.Active, EmployerId = employer3.Id, CreatedAt = DateTime.Now, ExpiryDate = DateTime.Now.AddDays(30) },
                new JobPost { Title = "Data Analyst", Description = "Phân tích dữ liệu kinh doanh, tạo báo cáo BI.", Salary = 16000000, Location = "Đà Nẵng", RequiredSkills = "Python, SQL, Power BI, Excel", JobType = "Full-time", Status = JobStatus.Active, EmployerId = employer1.Id, CreatedAt = DateTime.Now, ExpiryDate = DateTime.Now.AddDays(30) }
            };
            context.JobPosts.AddRange(jobs);
            context.SaveChanges();
        }
    }
}
