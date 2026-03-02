using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Domain.Parameters;
using LibraryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryDbContext _context;

        public BookRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<List<Book>> GetAllAsync(BookParameters parameters, CancellationToken cancellationToken)
        {
            var query = _context.Books.Include(b => b.Author).AsQueryable();
            if (!string.IsNullOrWhiteSpace(parameters.Title))
            {
                query = query.Where(b => b.Title.Contains(parameters.Title));
            }
            if (!string.IsNullOrWhiteSpace(parameters.Author))
            {
                query = query.Where(b => b.Author.FirstName.Contains(parameters.Author)
                    || b.Author.LastName.Contains(parameters.Author));
            }
            //pagination(skip an d take)
            var books = await query
                .OrderBy(b=>b.Title)
                .Skip((parameters.PageNumber - 1)*parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync(cancellationToken);

            return books;
        }

        public async Task<Book?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Books.Include(b => b.Author)
                                       .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        }

        public async Task<int> AddAsync(Book book, CancellationToken cancellationToken)
        {
            await _context.Books.AddAsync(book,cancellationToken);
            // savechanges ak argamovidzaxeb, UnitOfWork mixedavs
            return book.Id;
        }

        public Task UpdateAsync(Book book, CancellationToken cancellationToken)
        {
            _context.Books.Update(book);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var book = await _context.Books.FindAsync(new object[] { id },cancellationToken);
            if (book != null)
            {
                _context.Books.Remove(book);
            }
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Books.AnyAsync(b => b.Id == id,cancellationToken);
        }
    }
}