using JobBoard.Domain.Enums;
namespace JobBoard.Application.DTOs
{
    public class RegisterDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public UserRole Role { get; set; }
        public string? Skills { get; set; }
        public decimal ExpectedSalary { get; set; }
        public string? PreferredLocation { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanyDescription { get; set; }
        public string? CompanyAddress { get; set; }
    }
    public class LoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
    public class UserProfileDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public string? CompanyName { get; set; }
        public string? Skills { get; set; }
    }
}
