using LibraryManagement.Application.DTOs.Book;
using LibraryManagement.Domain.Parameters;

namespace LibraryManagement.Application.Interfaces
{
    public interface IBookService
    {
        
        Task<BookDto> GetBookByIdAsync(int id, CancellationToken cancellationToken);
        Task<int> CreateBookAsync(CreateBookDto bookDto, CancellationToken cancellationToken);//c
        Task<List<BookDto>> GetAllBooksAsync(BookParameters parameters, CancellationToken cancellationToken);//r
        Task UpdateBookAsync(int id, UpdateBookDto bookDto,CancellationToken cancellationToken);//u
        Task DeleteBookAsync(int id, CancellationToken cancellationToken);//d
        Task<bool> CheckAvailabilityAsync(int id, CancellationToken cancellationToken);
    }
}