using FluentValidation;
using JobBoard.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Validators
{
    public class CreateJobValidator : AbstractValidator<CreateJobDto>
    {
        public CreateJobValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Job title is required")
                .MaximumLength(100).WithMessage("Job title should not exceed 100 characters");

            RuleFor(x => x.CompanyName)
                .NotEmpty().WithMessage("Company name is required.");

             RuleFor(x => x.SalaryMin)
                .GreaterThan(0)
                .When(x => x.SalaryMin.HasValue)
                .WithMessage("The minimum wage should be more than zero");

             RuleFor(x => x.SalaryMax)
                .GreaterThan(0)
                .When(x => x.SalaryMax.HasValue)
                .WithMessage("The maximum wage should be more than zero");

             RuleFor(x => x)
                .Must(x =>
                    !x.SalaryMin.HasValue ||
                    !x.SalaryMax.HasValue ||
                    x.SalaryMax.Value >= x.SalaryMin.Value)
                .WithMessage("The maximum salary should not be less than the minimum salary");


             RuleFor(x => x.EmploymentType)
                .IsInEnum().WithMessage("The collaboration type is invalid");
        }
    }
}
