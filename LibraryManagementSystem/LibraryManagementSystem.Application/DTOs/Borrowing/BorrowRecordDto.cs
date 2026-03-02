namespace LibraryManagement.Application.DTOs.Borrowing
{
    public class BorrowRecordDto
    {
        public int Id { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public string PatronEmail { get; set; } = string.Empty;
        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}