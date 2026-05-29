using JobBoard.Application.DTOs;
using JobBoard.Application.Interfaces;
using JobBoard.Domain.Entities;
using JobBoard.Domain.Enums;
using JobBoard.Domain.Interfaces;

namespace JobBoard.Application.Services
{
    // SERVICE AuthService: Xác thực người dùng (MÔ PHỎNG bằng OOP)
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> LoginAsync(LoginDto loginDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);
            if (user == null || !user.VerifyPassword(loginDto.Password))
                return null;
            return user;
        }

        // ĐA HÌNH: Tạo Candidate hoặc Employer dựa trên Role
        public async Task<User> RegisterAsync(RegisterDto registerDto)
        {
            if (await _userRepository.EmailExistsAsync(registerDto.Email))
                throw new JobBoard.Domain.Exceptions.JobApplicationException(
                    "Email đã được sử dụng", "DUPLICATE_EMAIL");

            User user;
            if (registerDto.Role == UserRole.Candidate)
            {
                user = new Candidate
                {
                    Name = registerDto.Name, Email = registerDto.Email,
                    Phone = registerDto.Phone, Skills = registerDto.Skills,
                    ExpectedSalary = registerDto.ExpectedSalary,
                    PreferredLocation = registerDto.PreferredLocation
                };
            }
            else
            {
                user = new Employer
                {
                    Name = registerDto.Name, Email = registerDto.Email,
                    Phone = registerDto.Phone,
                    CompanyName = registerDto.CompanyName ?? "Chưa cập nhật",
                    CompanyDescription = registerDto.CompanyDescription,
                    CompanyAddress = registerDto.CompanyAddress
                };
            }
            user.Password = registerDto.Password;
            await _userRepository.AddAsync(user);
            return user;
        }

        public async Task<User?> GetCurrentUserAsync(int userId)
            => await _userRepository.GetByIdAsync(userId);
    }
}
