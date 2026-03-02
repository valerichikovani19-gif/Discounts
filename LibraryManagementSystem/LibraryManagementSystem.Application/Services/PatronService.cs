using LibraryManagement.Application.DTOs.Book;
using LibraryManagement.Application.DTOs.Patron;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using LibraryManagement.Domain.Interfaces;

namespace LibraryManagement.Application.Services
{
    public class PatronService : IPatronService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PatronService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<PatronDto>> GetAllPatronsAsync(CancellationToken cancellationToken)
        {
            var patrons = await _unitOfWork.Patrons.GetAllAsync(cancellationToken);
            return patrons.Select(p => new PatronDto
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Email = p.Email,
                MembershipDate = p.MembershipDate
            }).ToList();
        }

        public async Task<PatronDto> GetPatronByIdAsync(int id,CancellationToken cancellationToken)
        {
            var patron = await _unitOfWork.Patrons.GetByIdAsync(id, cancellationToken);
            if (patron == null)
                throw new NotFoundException($"Patron with ID {id} not found");

            return new PatronDto
            {
                Id = patron.Id,
                FirstName = patron.FirstName,
                LastName = patron.LastName,
                Email = patron.Email,
                MembershipDate = patron.MembershipDate
            };
        }

        public async Task<int> CreatePatronAsync(CreatePatronDto patronDto,CancellationToken cancellationToken)
        {
            var patron = new Patron
            {
                FirstName = patronDto.FirstName,
                LastName = patronDto.LastName,
                Email = patronDto.Email,
                MembershipDate = DateTime.UtcNow
            };

            await _unitOfWork.Patrons.AddAsync(patron,cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return patron.Id;
        }

        public async Task UpdatePatronAsync(int id, UpdatePatronDto patronDto,CancellationToken cancellationToken)
        {
            var patron = await _unitOfWork.Patrons.GetByIdAsync(id, cancellationToken);
            if (patron == null)
                throw new NotFoundException($"Patron with ID {id} not found");

            patron.FirstName = patronDto.FirstName;
            patron.LastName = patronDto.LastName;
            patron.Email = patronDto.Email;

            await _unitOfWork.Patrons.UpdateAsync(patron, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        public async Task<List<BookDto>> GetBorrowedBooksAsync(int patronId, CancellationToken cancellationToken)
        {
            var allRecords = await _unitOfWork.BorrowRecords.GetAllAsync(cancellationToken);
            //gavfiltr
            var activeLoans = allRecords
                .Where(r => r.PatronId == patronId && r.ReturnDate == null)
                .Select(r=>r.BookId)
                .ToList();
            //
            if (!activeLoans.Any()) return new List<BookDto>();
            //damerr wignis detalebi
            var books = new List<BookDto>();
            foreach (var bookId in activeLoans)
            {
                var book = await _unitOfWork.Books.GetByIdAsync(bookId, cancellationToken);
                if (book != null)
                {
                    //avtori nulia tuara shevamowmeb ak
                    string authorName = "Unknown";
                    if (book.Author != null)
                    {
                        authorName = $"{book.Author.FirstName} {book.Author.LastName}";
                    }
                    books.Add(new BookDto
                    {
                        Id = book.Id,
                        Title = book.Title,
                        ISBN = book.ISBN,
                        AuthorName = authorName,
                        PublicationYear = book.PublicationYear,
                        Description = book.Description,
                        Quantity = book.Quantity,
                        IsAvailable = book.Quantity > 0
                    });
                }
            }
            return books;
        }
        public async Task DeletePatronAsync(int id, CancellationToken cancellationToken)
        {
            var patron = await _unitOfWork.Patrons.GetByIdAsync(id, cancellationToken);
            if (patron == null)
                throw new NotFoundException($"Patron with ID {id} not found");

            await _unitOfWork.Patrons.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        
    }
}