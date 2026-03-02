namespace LibraryManagement.Application.DTOs.Book
{
    public class CreateBookDto
    {
        public string Title { get; set; } = string.Empty;
        public string ISBN { get; set; }= string.Empty;
        public int AuthorId { get; set; }
        public int PublicationYear { get; set; }
        public string Description { get; set; } = string.Empty;
        public string CoverImageUrl { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }
}