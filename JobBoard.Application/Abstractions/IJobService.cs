using JobBoard.Application.DTOs.Jobs;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Abstractions
{
    public interface IJobService
    {
        Task<IReadOnlyList<JobDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<JobDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<JobDto> CreateAsync(CreateJobDto createJobDto, CancellationToken cancellationToken = default);
        Task UpdateAsync(Guid id, UpdateJobDto updateJobDto, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
