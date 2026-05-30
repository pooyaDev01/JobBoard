using JobBoard.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Abstractions
{
    public interface IJobService
    {
        Task<IReadOnlyList<JobDto>> GetAllJobsAsync(CancellationToken cancellationToken = default);
        Task<JobDto?> GetJobByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<JobDto> CreateJobAsync(CreateJobDto createJobDto, CancellationToken cancellationToken = default);
        Task<bool> UpdateJobAsync(Guid id, UpdateJobDto updateJobDto, CancellationToken cancellationToken = default);
        Task<bool> DeleteJobAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
