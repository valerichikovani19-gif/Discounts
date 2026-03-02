using LibraryManagement.Application.DTOs.Book;
using LibraryManagement.Application.DTOs.Patron;

namespace LibraryManagement.Application.Interfaces
{
    public interface IPatronService
    {
        Task<List<PatronDto>> GetAllPatronsAsync(CancellationToken cancellationToken);
        Task<PatronDto> GetPatronByIdAsync(int id,CancellationToken cancellationToken);
        Task<int> CreatePatronAsync(CreatePatronDto patronDto,CancellationToken cancellationToken);
        Task UpdatePatronAsync(int id, UpdatePatronDto patronDto,CancellationToken cancellationToken);
        Task DeletePatronAsync(int id,CancellationToken cancellationToken);
        Task<List<BookDto>> GetBorrowedBooksAsync(int patronId,CancellationToken cancellationToken);
    }
}
