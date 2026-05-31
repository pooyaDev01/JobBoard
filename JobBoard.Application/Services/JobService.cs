using AutoMapper;
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
        private readonly IMapper _mapper;

        public JobService(IJobRepository jobRepository, IUnitOfWork unitOfWork, ICompanyRepository companyRepository, IMapper mapper)
        {
            _jobRepository = jobRepository;
            _unitOfWork = unitOfWork;
            _companyRepository = companyRepository;
            _mapper = mapper;
        }
        public async Task<IReadOnlyList<JobDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var jobs = await _jobRepository.GetAllAsync(cancellationToken);

            return _mapper.Map<List<JobDto>>(jobs);
        }
        public async Task<JobDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var job = await _jobRepository.GetByIdAsync(id, cancellationToken);

            if (job is null)
                return null;

            return job is null ? null : _mapper.Map<JobDto>(job);
        }
        public async Task<JobDto> CreateAsync(CreateJobDto dto, CancellationToken cancellationToken = default)
        {
            var company = await _companyRepository.GetByIdAsync(dto.CompanyId!.Value, cancellationToken);

            if (company is null)
                throw new InvalidOperationException("The specified company does not exist.");

            var job = _mapper.Map<Job>(dto);
            job.Id = Guid.NewGuid();
            job.CreatedAtUtc = DateTime.UtcNow;

            await _jobRepository.AddAsync(job, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var createdJob = await _jobRepository.GetByIdAsync(job.Id, cancellationToken);

            return _mapper.Map<JobDto>(createdJob);
        }
        public async Task<bool> UpdateAsync(Guid id, UpdateJobDto dto, CancellationToken cancellationToken = default)
        {
            var job = await _jobRepository.GetByIdAsync(id, cancellationToken);
            if (job is null)
                return false;

            var company = await _companyRepository.GetByIdAsync(dto.CompanyId!.Value, cancellationToken);
            if (company is null)
                return false;

            _mapper.Map(dto, job);
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
