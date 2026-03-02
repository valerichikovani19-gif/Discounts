using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Domain.Interfaces
{
    public interface IPatronRepository
    {
        Task<List<Patron>> GetAllAsync(CancellationToken cancellationToken);
        Task<Patron?> GetByIdAsync(int id,CancellationToken cancellationToken);
        Task<int> AddAsync(Patron patron,CancellationToken cancellationToken);
        Task UpdateAsync(Patron patron,CancellationToken cancellationToken);
        Task DeleteAsync(int id,CancellationToken cancellationToken);
        Task<bool> ExistsAsync(int id,CancellationToken cancellationToken);
    }
}