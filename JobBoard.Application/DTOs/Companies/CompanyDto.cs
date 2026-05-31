using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.DTOs.Companies
{
    public record CompanyDto(
        Guid Id,
        string Name,
        string Description,
        string Website,
        string Location,
        DateTime CreatedAtUtc,
        DateTime? UpdatedAtUtc
    );
}
