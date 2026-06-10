using JobBoard.Application.Common.Pagination;
using JobBoard.Application.Common.Queries;
using JobBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Abstractions
{
    public interface ICompanyRepository
    {
        Task<PagedResult<Company>> GetPagedAsync(CompanyQueryParameters parameters, CancellationToken cancellationToken = default);
        Task<Company?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task AddAsync(Company company, CancellationToken cancellationToken = default);
        void Update(Company company);
        void Delete(Company company);
    }
}
