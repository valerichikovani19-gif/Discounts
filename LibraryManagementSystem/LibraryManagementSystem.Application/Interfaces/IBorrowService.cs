using LibraryManagement.Application.DTOs.Borrowing;
using LibraryManagement.Domain.Parameters;

namespace LibraryManagement.Application.Interfaces
{
    public interface IBorrowService
    {
        Task<int> BorrowBookAsync(CreateBorrowRequestDto requestDto,CancellationToken cancellationToken);
        Task ReturnBookAsync(int borrowRecordId,CancellationToken cancellationToken);
        Task<List<BorrowRecordDto>> GetAllRecordsAsync(CancellationToken cancellationToken);
        Task<List<BorrowRecordDto>> GetOverdueRecordsAsync(CancellationToken cancellationToken);
        Task<List<BorrowRecordDto>> GetAllRecordsAsync(BorrowRecordParameters parameters,CancellationToken cancellationToken);
    }
}
