using JobBoard.Application.Abstractions;
using JobBoard.Application.DTOs;
using JobBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Services
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _jobRepository;
        private readonly IUnitOfWork _unitOfWork;

        public JobService(IJobRepository jobRepository, IUnitOfWork unitOfWork)
        {
            _jobRepository = jobRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<IReadOnlyList<JobDto>> GetAllJobsAsync(CancellationToken cancellationToken = default)
        {
            var jobs = await _jobRepository.GetAllAsync(cancellationToken);

            return jobs.Select(job => new JobDto(
                job.Id,
                job.Title,
                job.Description,
                job.CompanyName,
                job.Location,
                job.SalaryMin,
                job.SalaryMax,
                job.EmploymentType,
                job.IsActive,
                job.CreatedAtUtc,
                job.UpdatedAtUtc
            )).ToList();
        }
        public async Task<JobDto?> GetJobByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var job = await _jobRepository.GetByIdAsync(id, cancellationToken);

            if (job is null)
                return null;

            return new JobDto(
                job.Id,
                job.Title,
                job.Description,
                job.CompanyName,
                job.Location,
                job.SalaryMin,
                job.SalaryMax,
                job.EmploymentType,
                job.IsActive,
                job.CreatedAtUtc,
                job.UpdatedAtUtc
            );
        }
        public async Task<JobDto> CreateJobAsync(CreateJobDto createJobDto, CancellationToken cancellationToken = default)
        {
            var job = new Job
            {
                Id = Guid.NewGuid(),
                Title = createJobDto.Title,
                Description = createJobDto.Description,
                CompanyName = createJobDto.CompanyName,
                Location = createJobDto.Location,
                SalaryMin = createJobDto.SalaryMin,
                SalaryMax = createJobDto.SalaryMax,
                EmploymentType = createJobDto.EmploymentType,
                IsActive = true,
                CreatedAtUtc = DateTime.UtcNow,
                UpdatedAtUtc = null
            };

            await _jobRepository.AddAsync(job, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new JobDto(
                job.Id,
                job.Title,
                job.Description,
                job.CompanyName,
                job.Location,
                job.SalaryMin,
                job.SalaryMax,
                job.EmploymentType,
                job.IsActive,
                job.CreatedAtUtc,
                job.UpdatedAtUtc
            );
        }
        public async Task<bool> UpdateJobAsync(Guid id, UpdateJobDto updateJobDto, CancellationToken cancellationToken = default)
        {
            var job = await _jobRepository.GetByIdAsync(id, cancellationToken);

            if (job is null)
                return false;

            job.Title = updateJobDto.Title;
            job.Description = updateJobDto.Description;
            job.CompanyName = updateJobDto.CompanyName;
            job.Location = updateJobDto.Location;
            job.SalaryMin = updateJobDto.SalaryMin;
            job.SalaryMax = updateJobDto.SalaryMax;
            job.EmploymentType = updateJobDto.EmploymentType;
            job.IsActive = updateJobDto.IsActive;
            job.UpdatedAtUtc = DateTime.UtcNow;

            _jobRepository.Update(job);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
        public async Task<bool> DeleteJobAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var job = await _jobRepository.GetByIdAsync(id, cancellationToken);

            if (job is null)
                return false;

            _jobRepository.Delete(job);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
