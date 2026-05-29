using JobBoard.Domain.Entities;

namespace JobBoard.Domain.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<Candidate?> GetCandidateByIdAsync(int id);
        Task<Employer?> GetEmployerByIdAsync(int id);
        Task<bool> EmailExistsAsync(string email);
    }
}
