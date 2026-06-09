using AutoMapper;
using JobBoard.Application.Abstractions;
using JobBoard.Application.Common.Exceptions;
using JobBoard.Application.Common.Pagination;
using JobBoard.Application.Common.Queries;
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
        private readonly IMapper _mapper;

        public JobService(IJobRepository jobRepository, IUnitOfWork unitOfWork, ICompanyRepository companyRepository, IMapper mapper)
        {
            _jobRepository = jobRepository;
            _unitOfWork = unitOfWork;
            _companyRepository = companyRepository;
            _mapper = mapper;
        }
        public async Task<PagedResult<JobDto>> GetPagedAsync(JobQueryParameters parameters, CancellationToken cancellationToken = default)
        {
            var pagedJobs = await _jobRepository.GetPagedAsync(parameters, cancellationToken);

            var jobDtos = _mapper.Map<IReadOnlyList<JobDto>>(pagedJobs.Items);

            return new PagedResult<JobDto>
            {
                Items = jobDtos,
                PageNumber = pagedJobs.PageNumber,
                PageSize = pagedJobs.PageSize,
                TotalCount = pagedJobs.TotalCount
            };
        }
        public async Task<JobDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var job = await _jobRepository.GetByIdAsync(id, cancellationToken);

            if (job is null)
                throw new NotFoundException($"Job with ID {id} was not found.");

            return _mapper.Map<JobDto>(job);
        }
        public async Task<JobDto> CreateAsync(CreateJobDto dto, CancellationToken cancellationToken = default)
        {
            var company = await _companyRepository.GetByIdAsync(dto.CompanyId!.Value, cancellationToken);

            if (company is null)
                throw new NotFoundException($"The specified company with ID {dto.CompanyId} does not exist.");

            var job = _mapper.Map<Job>(dto);
            job.Id = Guid.NewGuid();
            job.CreatedAtUtc = DateTime.UtcNow;

            await _jobRepository.AddAsync(job, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var createdJob = await _jobRepository.GetByIdAsync(job.Id, cancellationToken);

            return _mapper.Map<JobDto>(createdJob);
        }
        public async Task UpdateAsync(Guid id, UpdateJobDto dto, CancellationToken cancellationToken = default)
        {
            var job = await _jobRepository.GetByIdAsync(id, cancellationToken);
            if (job is null)
                throw new NotFoundException($"Job with ID {id} was not found.");

            var company = await _companyRepository.GetByIdAsync(dto.CompanyId!.Value, cancellationToken);
            if (company is null)
                throw new NotFoundException($"The specified company with ID {dto.CompanyId} does not exist.");

            _mapper.Map(dto, job);
            job.UpdatedAtUtc = DateTime.UtcNow;

            _jobRepository.Update(job);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var job = await _jobRepository.GetByIdAsync(id, cancellationToken);
            if (job is null)
                throw new NotFoundException("Job not found to delete."); ;

            _jobRepository.Delete(job);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

    }
}
