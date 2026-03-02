using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Domain.Parameters;
using LibraryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Repositories
{
    public class BorrowRecordRepository : IBorrowRecordRepository
    {
        private readonly LibraryDbContext _context;

        public BorrowRecordRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<List<BorrowRecord>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.BorrowRecords
                .Include(br => br.Book)
                .Include(br => br.Patron)
                .ToListAsync(cancellationToken);
        }

        public async Task<BorrowRecord?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.BorrowRecords
                .Include(br => br.Book)
                .Include(br => br.Patron)
                .FirstOrDefaultAsync(br => br.Id == id, cancellationToken);
        }

        public async Task<int> AddAsync(BorrowRecord record, CancellationToken cancellationToken)
        {
            await _context.BorrowRecords.AddAsync(record, cancellationToken);
            return record.Id;
        }

        public Task UpdateAsync(BorrowRecord record,CancellationToken cancellationToken)
        {
            _context.BorrowRecords.Update(record);
            return Task.CompletedTask;
        }
         public async Task<List<BorrowRecord>> GetOverdueRecordsAsync(CancellationToken cancellationToken)
        {
            return await _context.BorrowRecords
                .Include(br => br.Book)
                .Include(br => br.Patron)
                .Where(br => br.Status == BorrowStatus.Overdue ||
                            (br.Status == BorrowStatus.Borrowed && br.DueDate < DateTime.Now))
                .ToListAsync(cancellationToken );
        }

        public async Task<List<BorrowRecord>> GetAllAsync(BorrowRecordParameters parameters, CancellationToken cancellationToken)
        {
            var query = _context.BorrowRecords
                .Include(br => br.Book)
                .Include(br => br.Patron)
                .AsQueryable();

            if (parameters.PatronId.HasValue)
            {
                query = query.Where(r=>r.PatronId== parameters.PatronId.Value);
            }
            if (parameters.BookId.HasValue)
            {
                query = query.Where(r => r.BookId== parameters.BookId.Value);
            }
            if (parameters.Status.HasValue)
            {
                query = query.Where(r => r.Status == parameters.Status.Value);
            }

            return await query
                .OrderByDescending(r => r.BorrowDate)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync(cancellationToken);
        }
    }
}