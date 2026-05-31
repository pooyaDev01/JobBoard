using JobBoard.Application.Abstractions;
using JobBoard.Application.DTOs.Jobs;
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
        private readonly ICompanyRepository _companyRepository;

        public JobService(IJobRepository jobRepository, IUnitOfWork unitOfWork, ICompanyRepository companyRepository)
        {
            _jobRepository = jobRepository;
            _unitOfWork = unitOfWork;
            _companyRepository = companyRepository;
        }
        public async Task<IReadOnlyList<JobDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var jobs = await _jobRepository.GetAllAsync(cancellationToken);

            return jobs.Select(job => new JobDto(
            job.Id,
            job.Title,
            job.Description,
            job.CompanyId,
            job.Company.Name,
            job.Location,
            job.SalaryMin,
            job.SalaryMax,
            job.EmploymentType,
            job.IsActive,
            job.CreatedAtUtc,
            job.UpdatedAtUtc
        )).ToList();
        }
        public async Task<JobDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var job = await _jobRepository.GetByIdAsync(id, cancellationToken);

            if (job is null)
                return null;

            return new JobDto(
            job.Id,
            job.Title,
            job.Description,
            job.CompanyId,
            job.Company.Name,
            job.Location,
            job.SalaryMin,
            job.SalaryMax,
            job.EmploymentType,
            job.IsActive,
            job.CreatedAtUtc,
            job.UpdatedAtUtc
            );
        }
        public async Task<JobDto> CreateAsync(CreateJobDto dto, CancellationToken cancellationToken = default)
        {
            var company = await _companyRepository.GetByIdAsync(dto.CompanyId!.Value, cancellationToken);

            if (company is null)
                throw new InvalidOperationException("The specified company does not exist.");

            var job = new Job
            {
                Id = Guid.NewGuid(),
                Title = dto.Title!,
                Description = dto.Description!,
                CompanyId = dto.CompanyId!.Value,
                Location = dto.Location!,
                SalaryMin = dto.SalaryMin,
                SalaryMax = dto.SalaryMax,
                EmploymentType = dto.EmploymentType!.Value,
                CreatedAtUtc = DateTime.UtcNow
            };

            await _jobRepository.AddAsync(job, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var createdJob = await _jobRepository.GetByIdAsync(job.Id, cancellationToken);

            return new JobDto(
                createdJob!.Id,
                createdJob.Title,
                createdJob.Description,
                createdJob.CompanyId,
                createdJob.Company.Name,
                createdJob.Location,
                createdJob.SalaryMin,
                createdJob.SalaryMax,
                createdJob.EmploymentType,
                createdJob.IsActive,
                createdJob.CreatedAtUtc,
                createdJob.UpdatedAtUtc
            );
        }
        public async Task<bool> UpdateAsync(Guid id, UpdateJobDto dto, CancellationToken cancellationToken = default)
        {
            var job = await _jobRepository.GetByIdAsync(id, cancellationToken);
            if (job is null)
                return false;

            var company = await _companyRepository.GetByIdAsync(dto.CompanyId!.Value, cancellationToken);
            if (company is null)
                throw new InvalidOperationException("The specified company does not exist.");

            job.Title = dto.Title!;
            job.Description = dto.Description!;
            job.CompanyId = dto.CompanyId!.Value;
            job.Location = dto.Location!;
            job.SalaryMin = dto.SalaryMin;
            job.SalaryMax = dto.SalaryMax;
            job.EmploymentType = dto.EmploymentType!.Value;
            job.UpdatedAtUtc = DateTime.UtcNow;

            _jobRepository.Update(job);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
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
