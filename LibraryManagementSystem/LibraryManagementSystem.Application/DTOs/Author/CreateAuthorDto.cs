using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Application.DTOs.Author
{
    public class CreateAuthorDto
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        public string Biography { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
    }
}
