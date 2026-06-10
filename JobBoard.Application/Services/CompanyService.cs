using AutoMapper;
using JobBoard.Application.Abstractions;
using JobBoard.Application.Common.Exceptions;
using JobBoard.Application.Common.Pagination;
using JobBoard.Application.Common.Queries;
using JobBoard.Application.DTOs.Companies;
using JobBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CompanyService(ICompanyRepository companyRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _companyRepository = companyRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<PagedResult<CompanyDto>> GetPagedAsync(CompanyQueryParameters parameters,CancellationToken cancellationToken = default)
        {
            var pagedCompanies = await _companyRepository.GetPagedAsync(parameters, cancellationToken);

            var companyDtos = _mapper.Map<IReadOnlyList<CompanyDto>>(pagedCompanies.Items);

            return new PagedResult<CompanyDto>
            {
                Items = companyDtos,
                PageNumber = pagedCompanies.PageNumber,
                PageSize = pagedCompanies.PageSize,
                TotalCount = pagedCompanies.TotalCount
            };
        }

        public async Task<CompanyDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var company = await _companyRepository.GetByIdAsync(id, cancellationToken);

            if (company is null)
                throw new NotFoundException($"Company with ID {id} was not found.");

            return _mapper.Map<CompanyDto>(company);
        }
        public async Task<CompanyDto> CreateAsync(CreateCompanyDto dto, CancellationToken cancellationToken = default)
        {
            var company = _mapper.Map<Company>(dto);
            company.Id = Guid.NewGuid();
            company.CreatedAtUtc = DateTime.UtcNow;

            await _companyRepository.AddAsync(company, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CompanyDto>(company);
        }

        public async Task UpdateAsync(Guid id, UpdateCompanyDto dto, CancellationToken cancellationToken = default)
        {
            var company = await _companyRepository.GetByIdAsync(id, cancellationToken);

            if (company is null)
                throw new NotFoundException($"Company with ID {id} was not found to update.");

            _mapper.Map(dto, company);
            company.UpdatedAtUtc = DateTime.UtcNow;

            _companyRepository.Update(company);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var company = await _companyRepository.GetByIdAsync(id, cancellationToken);

            if (company is null)
                throw new NotFoundException($"Company with ID {id} was not found to delete.");

            _companyRepository.Delete(company);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

    }
}
