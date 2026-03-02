using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Infrastructure.Data;

namespace LibraryManagement.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LibraryDbContext _context;

        public IBookRepository Books { get; private set; }
        public IAuthorRepository Authors { get; private set; }
        public IPatronRepository Patrons { get; private set; }
        public IBorrowRecordRepository BorrowRecords { get; private set; }

        public UnitOfWork(LibraryDbContext context)
        {
            _context = context;
            Books = new BookRepository(_context);
            Authors = new AuthorRepository(_context);
            Patrons = new PatronRepository(_context);
            BorrowRecords = new BorrowRecordRepository(_context);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}