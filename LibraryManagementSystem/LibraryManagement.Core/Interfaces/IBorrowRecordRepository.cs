using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Parameters;

namespace LibraryManagement.Domain.Interfaces
{
    public interface IBorrowRecordRepository
    {
        Task<List<BorrowRecord>> GetAllAsync(CancellationToken cancellationToken);
        Task<BorrowRecord?> GetByIdAsync(int id,CancellationToken cancellationToken);
        Task<int> AddAsync(BorrowRecord record,CancellationToken cancellationToken);
        Task UpdateAsync(BorrowRecord record , CancellationToken cancellationToken);
        Task<List<BorrowRecord>> GetOverdueRecordsAsync(CancellationToken cancellationToken);
        Task<List<BorrowRecord>> GetAllAsync(BorrowRecordParameters parameters,CancellationToken cancellationToken);
    }
}