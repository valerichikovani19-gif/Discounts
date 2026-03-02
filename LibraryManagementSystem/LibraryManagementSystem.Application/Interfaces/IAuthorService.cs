using LibraryManagement.Application.DTOs.Author;
using LibraryManagement.Application.DTOs.Book;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Interfaces
{
    public interface IAuthorService
    {
        Task<List<AuthorDto>> GetAllAuthorsAsync(CancellationToken cancellationToken);
        Task<AuthorDto> GetAuthorByIdAsync(int id,CancellationToken cancellationToken);
        Task<int> CreateAuthorAsync(CreateAuthorDto authorDto,CancellationToken cancellationToken);
        Task UpdateAuthorAsync(int id, UpdateAuthorDto authorDto,CancellationToken cancellationToken);
        Task DeleteAuthorAsync(int id,CancellationToken cancellationToken);
        Task<List<BookDto>> GetBooksByAuthorAsync(int authorId, CancellationToken cancellationToken);
    }
}
