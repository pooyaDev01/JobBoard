using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.DTOs.Companies
{
    public record UpdateCompanyDto(
    string? Name,
    string? Description,
    string? Website,
    string? Location);
}
