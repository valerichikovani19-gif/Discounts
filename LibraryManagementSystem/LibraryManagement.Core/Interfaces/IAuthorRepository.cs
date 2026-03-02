using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Domain.Interfaces
{
    public interface IAuthorRepository
    {
        Task<List<Author>> GetAllAsync(CancellationToken cancellationToken);
        Task<Author?> GetByIdAsync(int id,CancellationToken cancellationToken);
        Task<int> AddAsync(Author author,CancellationToken ct);
        Task UpdateAsync(Author author,CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken);
    }
}