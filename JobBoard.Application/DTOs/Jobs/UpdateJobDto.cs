using JobBoard.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.DTOs.Jobs
{
    public record UpdateJobDto(
    string? Title,
    string? Description,
    Guid? CompanyId,
    string? Location,
    decimal? SalaryMin,
    decimal? SalaryMax,
    EmploymentType? EmploymentType,
    bool IsActive);
    
}
