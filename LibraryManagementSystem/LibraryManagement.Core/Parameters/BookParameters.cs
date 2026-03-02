namespace LibraryManagement.Domain.Parameters
{
    public class BookParameters
    {
        // Default to page 1
        public int PageNumber { get; set; } = 1;

        // Default to 10 items per page
        private int _pageSize = 10;
        const int MaxPageSize = 50;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        // Search filtrebi
        public string? Title { get; set; }
        public string? Author { get; set; }
    }
}