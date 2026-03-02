using LibraryManagement.Application.Interfaces;
using LibraryManagement.Application.Services;
using LibraryManagement.Domain.Interfaces;

namespace LibraryManagement.Application.Manager
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IBookService> _bookService;
        private readonly Lazy<IAuthorService> _authorService;
        private readonly Lazy<IPatronService> _patronService;
        private readonly Lazy<IBorrowService> _borrowService;
        //vainjecteb Unitofworks da vawvdi servisebs.gamovikeneb Lazy inicializacias anu
        //sanam specifiurad armovitxovt ikamde ar sheikmneba BookService 
        public ServiceManager(IUnitOfWork unitOfWork)
        {
            _bookService = new Lazy<IBookService>(() => new BookService(unitOfWork));
            _authorService = new Lazy<IAuthorService>(() => new AuthorService(unitOfWork));
            _patronService = new Lazy<IPatronService>(() => new PatronService(unitOfWork));
            _borrowService = new Lazy<IBorrowService>(() => new BorrowService(unitOfWork));    
        }

        public IBookService BookService => _bookService.Value;
        public IAuthorService AuthorService => _authorService.Value;

        public IPatronService PatronService => _patronService.Value;
        public IBorrowService BorrowService => _borrowService.Value;
    }
}
