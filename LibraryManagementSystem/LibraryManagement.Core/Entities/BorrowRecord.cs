

using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Domain.Entities
{
    public class BorrowRecord
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int PatronId { get; set; }
        public Book? Book { get; set; }
        public Patron? Patron { get; set; }

        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public BorrowStatus Status { get; set; }
    }
}
