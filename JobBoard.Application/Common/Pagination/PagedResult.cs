using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Common.Pagination
{
    public class PagedResult<T>
    {
        public IReadOnlyList<T> Items { get; set; } = new List<T>();

        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public int TotalCount { get; set; }
        public int TotalPages =>
            PageSize <= 0 ? 0 : (int)Math.Ceiling((decimal)TotalCount / PageSize);

        public bool HasPerviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;

    }
}
