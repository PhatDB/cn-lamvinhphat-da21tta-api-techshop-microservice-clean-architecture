namespace BuildingBlocks.Extensions
{
    public class PagedResult<T>
    {
        public PagedResult(
            List<T> items, int totalItems, int? pageNumber = null, int? pageSize = null)
        {
            Data = items;
            TotalItems = totalItems;
            PageNumber = Math.Max(pageNumber ?? 1, 1);
            PageSize = Math.Max(pageSize ?? 10, 1);
        }

        public List<T> Data { get; set; }
        public int TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
    }
}