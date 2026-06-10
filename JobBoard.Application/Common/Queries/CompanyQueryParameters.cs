using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Common.Queries
{
    public class CompanyQueryParameters
    {
        public string? Search {  get; set; }
        public string? Location { get; set; }

        public string? SortBy { get; set; } = "createdatutc";
        public string? SortDirection { get; set; } = "desc";

        public int? PageNumber { get; set; } = 1;
        public int? PageSize { get; set; } = 10;
    }
}
