using JobBoard.Application.Abstractions;
using JobBoard.Application.Common.Pagination;
using JobBoard.Application.Common.Queries;
using JobBoard.Domain.Entities;
using JobBoard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace JobBoard.Infrastructure.Repositories
{
    public class JobRepository : IJobRepository
    {
        private readonly JobBoardDbContext _context;

        public JobRepository(JobBoardDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Job>> GetPagedAsync(JobQueryParameters parameters, CancellationToken cancellationToken = default)
        {
            IQueryable<Job> query = _context.Jobs
                 .Include(x => x.Company)
                 .AsQueryable();

            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                var search = parameters.Search.Trim();

                query = query.Where(x =>
                x.Title.Contains(search)||
                x.Description.Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(parameters.Location))
            {
                var location = parameters.Location.Trim();

                query = query.Where(x => x.Location.Contains(location));
            }

            if (parameters.CompanyId.HasValue)
            {
                query = query.Where(x => x.CompanyId == parameters.CompanyId.Value);
            }

            if (parameters.EmploymentType.HasValue)
            {
                query = query.Where(x => x.EmploymentType == parameters.EmploymentType.Value);
            }

            if (parameters.IsActive.HasValue)
            {
                query = query.Where(x => x.IsActive == parameters.IsActive.Value);
            }

            if (parameters.SalaryMin.HasValue)
            {
                query = query.Where(x => x.SalaryMin >= parameters.SalaryMin.Value);
            }

            if (parameters.SalaryMax.HasValue)
            {
                query = query.Where(x => x.SalaryMax <= parameters.SalaryMax.Value);
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

            return new PagedResult<Job>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<Job?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Jobs
                .AsNoTracking()
                .Include(x => x.Company)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task AddAsync(Job job, CancellationToken cancellationToken = default)
        {
            await _context.Jobs.AddAsync(job, cancellationToken);
        }

        public void Update(Job job)
        {
            _context.Jobs.Update(job);
        }
        public void Delete(Job job)
        {
            _context.Jobs.Remove(job);
        }

        private IQueryable<Job> ApplySorting(IQueryable<Job> query, string? sortBy, string? sortDirection)
        {
            var isDescending = sortDirection?.Equals("desc", StringComparison.OrdinalIgnoreCase) ?? true;

            return sortBy?.Trim().ToLower() switch
            {
                "title" => isDescending
                ? query.OrderByDescending(x => x.Title)
                : query.OrderBy(x => x.Title),

                "location" => isDescending
                ? query.OrderByDescending(x => x.Location)
                : query.OrderBy(x => x.Location),

                "salarymin" => isDescending
                    ? query.OrderByDescending(x => x.SalaryMin)
                    : query.OrderBy(x => x.SalaryMin),

                "salarymax" => isDescending
                    ? query.OrderByDescending(x => x.SalaryMax)
                    : query.OrderBy(x => x.SalaryMax),

                "employmenttype" => isDescending
                    ? query.OrderByDescending(x => x.EmploymentType)
                    : query.OrderBy(x => x.EmploymentType),

                "createdatutc" => isDescending
                    ? query.OrderByDescending(x => x.CreatedAtUtc)
                    : query.OrderBy(x => x.CreatedAtUtc),

                _ => query.OrderByDescending(x => x.CreatedAtUtc)
            };
        }

    }

}
