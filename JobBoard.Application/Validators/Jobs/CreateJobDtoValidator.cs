using FluentValidation;
using JobBoard.Application.DTOs.Jobs;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Validators.Jobs
{
    public class CreateJobDtoValidator : AbstractValidator<CreateJobDto>
    {
        public CreateJobDtoValidator()
        {
            RuleFor(x => x.Title)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Job title is required")
                .MaximumLength(100).WithMessage("Job title should not exceed 100 characters");

            RuleFor(x => x.Description)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("The description field cannot be empty")
                .MaximumLength(1000).WithMessage("Description should not exceed 1000 characters");

            RuleFor(x => x.CompanyId)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("CompanyId is required.")
            .Must(id => id != Guid.Empty).WithMessage("CompanyId is required.");

            RuleFor(x => x.Location)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Location is required.")
                .MaximumLength(200).WithMessage("Location should not exceed 200 characters");

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
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("Employment type is required.")
                .IsInEnum().WithMessage("The Employment type is invalid");
        }
    }
}
