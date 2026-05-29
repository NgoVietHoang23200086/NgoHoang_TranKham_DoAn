using JobBoard.Application.DTOs;
using JobBoard.Domain.Entities;
namespace JobBoard.Application.Interfaces
{
    public interface IAuthService
    {
        Task<User?> LoginAsync(LoginDto loginDto);
        Task<User> RegisterAsync(RegisterDto registerDto);
        Task<User?> GetCurrentUserAsync(int userId);
    }
}
