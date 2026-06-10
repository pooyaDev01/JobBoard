using FluentValidation;
using JobBoard.Application.Common.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Validators.Companies
{
    public class CompanyQueryParametersValidator : AbstractValidator<CompanyQueryParameters>
    {
        private static readonly string[] AllowedSortFields =
        {
            "name",
            "location",
            "createdatutc"
        };
        public CompanyQueryParametersValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1);

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100);

            RuleFor(x => x.SortDirection)
                .Must(x => string.IsNullOrWhiteSpace(x) ||
                string.Equals(x, "desc", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(x, "asc", StringComparison.OrdinalIgnoreCase))
                .WithMessage("SortDirection must be either 'asc' or 'desc'.");

            RuleFor(x => x.SortBy)
                .Must( x => string.IsNullOrWhiteSpace(x) ||
                AllowedSortFields.Contains(x,StringComparer.OrdinalIgnoreCase))
                .WithMessage("Invalid sortBy field.");
                
        }
    }
}
