using LibraryManagement.Application.DTOs.Book;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Domain.Parameters;

namespace LibraryManagement.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;
        //ctor injection
        public BookService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CheckAvailabilityAsync(int id, CancellationToken cancellationToken)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(id, cancellationToken);
            if (book == null)
                throw new Exception($"Book with ID {id} not found");

            return book.Quantity > 0;
        }

        public async Task<int> CreateBookAsync(CreateBookDto bookDto, CancellationToken cancellationToken)
        {
            var book = new Book
            {
                Title = bookDto.Title,
                ISBN = bookDto.ISBN,
                AuthorId = bookDto.AuthorId,
                PublicationYear = bookDto.PublicationYear,
                Description = bookDto.Description,
                CoverImageUrl = bookDto.CoverImageUrl,
                Quantity = bookDto.Quantity
            };

            await _unitOfWork.Books.AddAsync(book, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return book.Id;
        }

        public async Task<List<BookDto>> GetAllBooksAsync(BookParameters parameters, CancellationToken cancellationToken)
        {
            var books = await _unitOfWork.Books.GetAllAsync(parameters, cancellationToken);
            var bookDtos = books.Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                ISBN = b.ISBN,
                PublicationYear = b.PublicationYear,
                Description = b.Description,
                Quantity = b.Quantity,
                AuthorName = b.Author != null ? $"{b.Author.FirstName} {b.Author.LastName}" : "Unknown",

                IsAvailable = b.Quantity > 0
            }).ToList();
            return bookDtos;
        }

        public async Task<BookDto> GetBookByIdAsync(int id, CancellationToken cancellationToken)
        {
            var b = await _unitOfWork.Books.GetByIdAsync(id, cancellationToken);
            if (b == null)
                throw new NotFoundException($"Book with ID {id} not found");

            return new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                ISBN = b.ISBN,
                PublicationYear = b.PublicationYear,
                Description = b.Description,
                Quantity = b.Quantity,
                AuthorName = b.Author != null ? $"{b.Author.FirstName} {b.Author.LastName}" : "Unknown",
                IsAvailable = b.Quantity > 0
            };
        }

        public async Task UpdateBookAsync(int id, UpdateBookDto bookDto,CancellationToken cancellationToken)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(id, cancellationToken);
            if (book == null)
                throw new NotFoundException($"Book with ID {id} not found");

            book.Title = bookDto.Title;
            book.ISBN = bookDto.ISBN;
            book.AuthorId = bookDto.AuthorId;
            book.PublicationYear = bookDto.PublicationYear;
            book.Description = bookDto.Description;
            book.CoverImageUrl = bookDto.CoverImageUrl;
            book.Quantity = bookDto.Quantity;

            await _unitOfWork.Books.UpdateAsync(book, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

        }
        public async Task DeleteBookAsync(int id, CancellationToken cancellationToken)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(id, cancellationToken);
            if (book == null)
                throw new NotFoundException($"Book with ID {id} not found");

            await _unitOfWork.Books.DeleteAsync(id, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
