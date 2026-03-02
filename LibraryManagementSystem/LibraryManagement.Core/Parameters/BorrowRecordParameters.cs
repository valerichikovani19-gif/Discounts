using LibraryManagement.Domain.Enums;

namespace LibraryManagement.Domain.Parameters
{
    public class BorrowRecordParameters
    {
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;
        const int MaxPageSize = 50;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        // Filterebi
        public int? PatronId { get; set; }       // Userit 
        public int? BookId { get; set; }         // wignit
        public BorrowStatus? Status { get; set; } // Statusit 
    }
}