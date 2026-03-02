namespace LibraryManagement.Application.DTOs.Book
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public int PublicationYear { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public bool IsAvailable { get; set; }
    }
}