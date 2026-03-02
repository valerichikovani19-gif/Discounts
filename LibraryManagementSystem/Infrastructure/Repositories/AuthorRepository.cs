using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly LibraryDbContext _context;

        public AuthorRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<List<Author>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Authors.ToListAsync(cancellationToken);
        }

        public async Task<Author?> GetByIdAsync(int id,CancellationToken cancellationToken)
        {
            return await _context.Authors.Include(a => a.Books)
                                         .FirstOrDefaultAsync(a => a.Id == id,cancellationToken);
        }

        public async Task<int> AddAsync(Author author, CancellationToken cancellationToken)
        {
            await _context.Authors.AddAsync(author, cancellationToken);
            return author.Id;
        }

        public Task UpdateAsync(Author author, CancellationToken cancellationToken)
        {
            _context.Authors.Update(author);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var author = await _context.Authors.FindAsync(new object[] { id}, cancellationToken);
            if (author != null)
            {
                _context.Authors.Remove(author);
            }
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Authors.AnyAsync(a => a.Id == id, cancellationToken);
        }
    }
}