using JobBoard.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.DTOs
{
    public class JobDto(
    Guid Id,
    string Title,
    string Description,
    string CompanyName,
    string Location,
    decimal? SalaryMin,
    decimal? SalaryMax,
    EmploymentType EmploymentType,
    bool IsActive,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc);

}
