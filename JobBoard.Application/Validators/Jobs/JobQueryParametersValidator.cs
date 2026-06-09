using FluentValidation;
using JobBoard.Application.Common.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Validators.Jobs
{
    public class JobQueryParametersValidator : AbstractValidator<JobQueryParameters>
    {
        private static readonly string[] AllowedSortFields =
        {
            "title",
            "location",
            "salaryMin",
            "salaryMax",
            "createdAtUtc",
            "employmentType"
        };
        public JobQueryParametersValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1);

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100);

            RuleFor(x => x.SortDirection)
                .Must(x => string.IsNullOrWhiteSpace(x) ||
                x.Equals("asc", StringComparison.OrdinalIgnoreCase)||
                x.Equals("desc", StringComparison.OrdinalIgnoreCase))
                .WithMessage("SortDirection must be either 'asc' or 'desc'.");

            RuleFor(x => x.SortBy)
                .Must(x => string.IsNullOrWhiteSpace(x) ||
                AllowedSortFields.Contains(x, StringComparer.OrdinalIgnoreCase))
                .WithMessage("Invalid SortBy field.");
        }
    }
}
