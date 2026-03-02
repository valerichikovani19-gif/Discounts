using LibraryManagement.Application.DTOs.Borrowing;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using LibraryManagement.Domain.Exceptions;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Domain.Parameters;

namespace LibraryManagement.Application.Services
{
    public class BorrowService : IBorrowService
    {
        private readonly IUnitOfWork _unitOfWork;
        public BorrowService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<int> BorrowBookAsync(CreateBorrowRequestDto requestDto,CancellationToken cancellationToken)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(requestDto.BookId, cancellationToken);
            
            if (book == null)
                throw new NotFoundException($"Book with ID {requestDto.BookId} not found"); 
            //
            var patron = await _unitOfWork.Patrons.GetByIdAsync(requestDto.PatronId, cancellationToken);
            if (patron == null)
                throw new NotFoundException($"Patron with ID {requestDto.PatronId} not found");

            if (book.Quantity <= 0)
                throw new BusinessRuleException($"Book '{book.Title}' is currently unavailable (Quantity is 0).");

            //record creating
            var borrowRecord = new BorrowRecord
            {
                BookId = requestDto.BookId,
                PatronId = requestDto.PatronId,
                BorrowDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(14), // vada ori kvira
                Status = BorrowStatus.Borrowed
            };
            book.Quantity -= 1;
            //adding record and updating book 
            await _unitOfWork.BorrowRecords.AddAsync(borrowRecord, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return borrowRecord.Id;

        }

        public async Task<List<BorrowRecordDto>> GetAllRecordsAsync(CancellationToken cancellationToken)
        {
            var records = await _unitOfWork.BorrowRecords.GetAllAsync(cancellationToken);

            return records.Select(r => new BorrowRecordDto
            {
                Id = r.Id,
                BookTitle = r.Book != null ? r.Book.Title : "Unknown",
                PatronEmail = r.Patron != null ? r.Patron.Email : "Unknown",
                BorrowDate = r.BorrowDate,
                DueDate = r.DueDate,
                ReturnDate = r.ReturnDate,
                Status = r.Status.ToString()
            }).ToList();
        }

        public async Task ReturnBookAsync(int borrowRecordId, CancellationToken cancellationToken)
        {
            var record = await _unitOfWork.BorrowRecords.GetByIdAsync(borrowRecordId,cancellationToken);
            if (record == null)
                throw new NotFoundException($"Borrow record with ID {borrowRecordId} not found");

            if (record.Status == BorrowStatus.Returned)
                throw new BusinessRuleException("This book has already been returned");
            //update record
            record.ReturnDate = DateTime.UtcNow;
            record.Status = BorrowStatus.Returned;
            // increase quantity

            var book = await _unitOfWork.Books.GetByIdAsync(record.BookId, cancellationToken);
            if (book != null)
            {
                book.Quantity += 1;
            }
            await _unitOfWork.BorrowRecords.UpdateAsync(record, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

        }
        public async Task<List<BorrowRecordDto>> GetOverdueRecordsAsync(CancellationToken cancellationToken)
        {
            var overdueRecords = await _unitOfWork.BorrowRecords.GetOverdueRecordsAsync(cancellationToken);
            //map
            return overdueRecords.Select(r => new BorrowRecordDto
            {
                Id = r.Id,
                BookTitle = r.Book != null ? r.Book.Title : "Unknown",
                PatronEmail = r.Patron != null ? r.Patron.Email : "Unknown",
                BorrowDate = r.BorrowDate,
                DueDate = r.DueDate,
                ReturnDate = r.ReturnDate,
                Status = "Overdue" // expliciturad dalabeldeba es ak
            }).ToList();

        }
        public async Task<List<BorrowRecordDto>> GetAllRecordsAsync(BorrowRecordParameters parameters, CancellationToken cancellationToken)
        {
            var records = await _unitOfWork.BorrowRecords.GetAllAsync(parameters, cancellationToken);

            // mapireba
            return records.Select(r => new BorrowRecordDto
            {
                Id = r.Id,
                BookTitle = r.Book != null ? r.Book.Title : "Unknown",
                PatronEmail = r.Patron != null ? r.Patron.Email : "Unknown",
                BorrowDate = r.BorrowDate,
                DueDate = r.DueDate,
                ReturnDate = r.ReturnDate,
                Status = r.Status.ToString()
            }).ToList();
        }
    }
}
