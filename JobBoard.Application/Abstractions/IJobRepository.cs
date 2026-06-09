using JobBoard.Application.Common.Pagination;
using JobBoard.Application.Common.Queries;
using JobBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Abstractions
{
    public interface IJobRepository
    {
        Task<PagedResult<Job>> GetPagedAsync(JobQueryParameters parameters, CancellationToken cancellationToken = default);
        Task<Job?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task AddAsync(Job job, CancellationToken cancellationToken = default);
        void Update(Job job);
        void Delete(Job job);
    }
}
