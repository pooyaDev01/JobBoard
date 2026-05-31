using FluentValidation;
using JobBoard.Application.DTOs.Companies;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Validators.Companies
{
    public class CreateCompanyDtoValidator : AbstractValidator<CreateCompanyDto>
    {
        public CreateCompanyDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Company name is required.")
                .MaximumLength(200).WithMessage("Company name must not exceed 200 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description about company is required.")
                .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters.");

            RuleFor(x => x.Website)
                .NotEmpty().WithMessage("Website of company is required.")
                .MaximumLength(500).WithMessage("Website must not exceed 500 characters.");

            RuleFor(x => x.Location)
                .NotEmpty().WithMessage("Location of company is required.")
                .MaximumLength(200).WithMessage("Location must not exceed 200 characters.");

        }
    }
}
