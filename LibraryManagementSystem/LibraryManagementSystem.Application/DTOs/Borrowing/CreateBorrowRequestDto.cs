using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Application.DTOs.Borrowing
{
    public class CreateBorrowRequestDto
    {
        [Required]
        public int BookId { get; set; }

        [Required]
        public int PatronId { get; set; }
    }
}