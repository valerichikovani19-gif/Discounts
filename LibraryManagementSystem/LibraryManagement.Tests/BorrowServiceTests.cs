using LibraryManagement.Application.DTOs.Borrowing;
using LibraryManagement.Application.Services;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exceptions;
using LibraryManagement.Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Tests
{
    public class BorrowServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly BorrowService _borrowService;

        public BorrowServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _borrowService = new BorrowService(_mockUnitOfWork.Object);
        }
        [Fact]
        public async Task BorrowBookAsync_shouldThrowNotFoundException_WhenBookDoesNotExist()
        {
            //arrange

            var request = new CreateBorrowRequestDto { BookId = 999, PatronId = 1 };
            //setup
            _mockUnitOfWork.Setup(u => u.Books.GetByIdAsync(999, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Book?)null);

            //act and assert

            await Assert.ThrowsAsync<NotFoundException>(() =>
            _borrowService.BorrowBookAsync(request, CancellationToken.None));
        }
        [Fact]
        public async Task BorrowBookAsync_ShouldThrowBusinessRuleException_WhenBookIsUnavailable()
        {
            var bookId = 1;
            var patronId = 1;

            var unavailableBook = new Book { Id = bookId, Title = "Lost Book", Quantity = 0 };
            var patron = new Patron { Id = patronId };

            _mockUnitOfWork.Setup(u => u.Books.GetByIdAsync(bookId, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(unavailableBook);
            _mockUnitOfWork.Setup(u => u.Patrons.GetByIdAsync(patronId, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(patron);

            var request = new CreateBorrowRequestDto { BookId = bookId, PatronId = patronId };

            await Assert.ThrowsAsync<BusinessRuleException>(() =>
                _borrowService.BorrowBookAsync(request, CancellationToken.None));
        }
        [Fact]
        public async Task BorrowBookAsync_ShouldReduceQuantity_WhenSuccessful()
        {
            var bookId = 1;
            var patronId = 1;
            var initialQuantity = 5;

            var book = new Book { Id = bookId, Title = "Great Book", Quantity = initialQuantity };
            var patron = new Patron { Id = patronId, FirstName = "John", LastName = "Doe" };

            _mockUnitOfWork.Setup(u => u.Books.GetByIdAsync(bookId, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(book);
            _mockUnitOfWork.Setup(u => u.Patrons.GetByIdAsync(patronId, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(patron);

            _mockUnitOfWork.Setup(u => u.BorrowRecords.AddAsync(It.IsAny<BorrowRecord>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync(1);
            var request = new CreateBorrowRequestDto { BookId = bookId, PatronId = patronId };

            await _borrowService.BorrowBookAsync(request, CancellationToken.None);

            Assert.Equal(initialQuantity - 1, book.Quantity);

            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
