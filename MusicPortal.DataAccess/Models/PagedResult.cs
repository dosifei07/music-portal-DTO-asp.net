namespace MusicPortal.DataAccess.Models
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; } = 1;
        public int PageIndex { get; set; }
        public int PageSize { get; set; } = 10;

        public int TotalPages => PageSize <= 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);
        public bool HasPrevious => Page > 1;
        public bool HasNext => Page < TotalPages;

        public PagedResult() { }

        public PagedResult(IEnumerable<T> items, int totalCount, int page, int pageSize)
        {
            Items = items.ToList();
            TotalCount = totalCount;
            Page = page;
            PageSize = pageSize;
        }
    }
}
