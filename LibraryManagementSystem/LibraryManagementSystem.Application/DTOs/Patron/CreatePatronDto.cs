using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.DTOs.Patron
{
    public class CreatePatronDto
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty
;
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
