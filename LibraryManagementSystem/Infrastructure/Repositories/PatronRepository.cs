using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Repositories
{
    public class PatronRepository : IPatronRepository
    {
        private readonly LibraryDbContext _context;

        public PatronRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<List<Patron>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Patrons.ToListAsync(cancellationToken);
        }

        public async Task<Patron?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Patrons.Include(p => p.BorrowRecords)
                                         .FirstOrDefaultAsync(p => p.Id == id,cancellationToken);
        }

        public async Task<int> AddAsync(Patron patron, CancellationToken cancellationToken)
        {
            await _context.Patrons.AddAsync(patron, cancellationToken);
            return patron.Id;
        }

        public Task UpdateAsync(Patron patron, CancellationToken cancellationToken)
        {
            _context.Patrons.Update(patron);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var patron = await _context.Patrons.FindAsync(id, cancellationToken);
            if (patron != null)
            {
                _context.Patrons.Remove(patron);
            }
        }

        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Patrons.AnyAsync(p => p.Id == id, cancellationToken);
        }
    }
}
