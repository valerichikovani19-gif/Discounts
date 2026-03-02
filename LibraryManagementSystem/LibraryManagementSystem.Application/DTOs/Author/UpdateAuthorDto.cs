using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.DTOs.Author
{
    public class UpdateAuthorDto
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [MaxLength()]
        public string LastName { get; set; } = string.Empty;
        public string Biography { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; } 
    }
}
