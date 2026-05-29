// ==========================================================
// GENERIC INTERFACE IRepository: Repository Pattern
// 
// === GIẢI THÍCH DESIGN PATTERN ===
// Repository Pattern tách biệt logic truy cập dữ liệu khỏi business logic.
// Generic Interface sử dụng Type Parameter <T> để tái sử dụng cho nhiều entity.
// Constraint "where T : class" đảm bảo T phải là reference type.
// ==========================================================
namespace JobBoard.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
