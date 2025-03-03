namespace BuildingBlocks.Extensions
{
    public class PaginationOption
    {
        public PaginationOption(
            string? sortBy = null, bool? isDescending = null, int? pageNumber = null,
            int? pageSize = null)
        {
            SortBy = sortBy ?? string.Empty;
            IsDescending = isDescending ?? false;
            PageNumber = Math.Max(pageNumber ?? 1, 1);
            PageSize = Math.Max(pageSize ?? 10, 1);
        }

        public string? SortBy { get; set; }
        public bool? IsDescending { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
    }
}