using JobBoard.Application.Abstractions;
using JobBoard.Application.Common.Pagination;
using JobBoard.Application.Common.Queries;
using JobBoard.Domain.Entities;
using JobBoard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Infrastructure.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly JobBoardDbContext _context;

        public CompanyRepository(JobBoardDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Company>> GetPagedAsync(CompanyQueryParameters parameters, CancellationToken cancellationToken = default)
        {
            IQueryable<Company> query = _context.Companies
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                var search = parameters.Search.Trim();

                query = query.AsNoTracking()
                    .Where(x => x.Name.Contains(search)||
                    x.Description.Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(parameters.Location))
            {
                var location = parameters.Location.Trim();

                query = query.AsNoTracking()
                    .Where(x => x.Location.Contains(location));
            }

            query = ApplySorting(query, parameters.SortBy, parameters.SortDirection);

            var totalCount = await query.CountAsync();

            var pageNumber = Math.Max(1, parameters.PageNumber ?? 1);
            var pageSize = Math.Min(100, parameters.PageSize ?? 10);

            var skip = (pageNumber - 1) * pageSize;

            var items = await query
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Company>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<Company?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Companies
              .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task AddAsync(Company company, CancellationToken cancellationToken = default)
        {
            await _context.Companies.AddAsync(company, cancellationToken);
        }

        public void Update(Company company)
        {
            _context.Companies.Update(company);
        }

        public void Delete(Company company)
        {
            _context.Companies.Remove(company);
        }

        private IQueryable<Company> ApplySorting(IQueryable<Company> query, string? sortBy, string? sortDirection)
        {
            var isDescending = sortDirection?.Equals("desc", StringComparison.OrdinalIgnoreCase) ?? true;

            return sortBy?.Trim().ToLower() switch
            {
                "name" => isDescending
                ? query.OrderByDescending(x => x.Name)
                : query.OrderBy(x => x.Name),

                "location" => isDescending
                ? query.OrderByDescending(x => x.Location)
                : query.OrderBy(x => x.Location),

                "createdatutc" => isDescending
                ? query.OrderByDescending(x => x.CreatedAtUtc)
                : query.OrderBy(x => x.CreatedAtUtc),

                _ => query.OrderByDescending(x=> x.CreatedAtUtc)
            };
    

        }

    }
}
