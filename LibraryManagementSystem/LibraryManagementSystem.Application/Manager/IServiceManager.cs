using LibraryManagement.Application.Interfaces;

namespace LibraryManagement.Application.Manager
{
    public interface IServiceManager
    {
        IBookService BookService { get; }
        IAuthorService AuthorService { get; }
        IPatronService PatronService { get; }
        IBorrowService BorrowService { get; }
    }
}