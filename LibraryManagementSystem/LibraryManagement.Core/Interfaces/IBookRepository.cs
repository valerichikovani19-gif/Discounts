
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Parameters;

namespace LibraryManagement.Domain.Interfaces
{
    public interface IBookRepository
    {
        //Task<List<Book>> GetAllAsync();
        Task<List<Book>> GetAllAsync(BookParameters parameters,CancellationToken cancellationToken);
        Task<Book?> GetByIdAsync(int id,CancellationToken cancellationToken);
        Task<int> AddAsync(Book book,CancellationToken cancellationToken);
        Task UpdateAsync(Book book,CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
        Task<bool> ExistsAsync(int id, CancellationToken cancellationToken);

    }
}
