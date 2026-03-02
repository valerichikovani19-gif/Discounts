using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.DTOs.Book
{
    public class UpdateBookDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(13)]
        public string ISBN { get; set; }
        [Required]
        public int AuthorId {  get; set; }
        
        public int PublicationYear {  get; set; }
        public string Description {  get; set; }
        public string CoverImageUrl {  get; set; }
        [Range(0,int.MaxValue)]
        public int Quantity {  get; set; }
    }
}
