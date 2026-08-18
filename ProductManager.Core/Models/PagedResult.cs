namespace ProductManager.Core.Models
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

        public static PagedResult<T> Create(IEnumerable<T> items, int totalCount, int currentPage, int pageSize)
        {
            return new PagedResult<T>
            {
                Items = items,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                CurrentPage = currentPage,
                PageSize = pageSize
            };
        }
    }
}
