namespace BusinessLayer.Common
{
    public class PaginatedResult<T>
    {
        public IReadOnlyList<T> Items { get; }
        public int PageIndex { get; }
        public int PageSize { get; }
        public int TotalCount { get; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        public PaginatedResult(
            IReadOnlyList<T> items,
            int pageIndex,
            int pageSize,
            int totalCount)
        {
            Items = items;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
        }
    }
}
