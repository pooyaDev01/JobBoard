using JobBoard.Application.DTOs.Companies;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Abstractions
{
    public interface ICompanyService
    {
        Task<IReadOnlyList<CompanyDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<CompanyDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<CompanyDto> CreateAsync(CreateCompanyDto dto, CancellationToken cancellationToken = default);
        Task UpdateAsync(Guid id, UpdateCompanyDto dto, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
