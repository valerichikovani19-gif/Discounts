namespace LibraryManagement.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IBookRepository Books { get; }
        IAuthorRepository Authors { get; }
        IPatronRepository Patrons { get; }
        IBorrowRecordRepository BorrowRecords { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
