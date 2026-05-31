using JobBoard.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JobBoard.Application.DTOs.Jobs
{
    public record CreateJobDto(
    string? Title,
    string? Description,
    Guid? CompanyId,
    string? Location,
    decimal? SalaryMin,
    decimal? SalaryMax,
    EmploymentType? EmploymentType);
}