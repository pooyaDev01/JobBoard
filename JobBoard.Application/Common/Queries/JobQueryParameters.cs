using JobBoard.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Common.Queries
{
    public class JobQueryParameters
    {
        public string? Search {  get; set; }
        public string? Location { get; set; }
        public Guid? CompanyId { get; set; }
        public EmploymentType? EmploymentType { get; set; }
        public bool? IsActive { get; set; } = true;

        public decimal? SalaryMin { get; set; }
        public decimal? SalaryMax { get; set; }

        public string? SortBy { get; set; } = "CreatedAtUtc";
        public string? SortDirection { get; set; } = "desc";

        public int? PageNumber { get; set; } = 1;
        public int? PageSize { get; set;} = 10;
    }
}
