using AutoMapper;
using JobBoard.Application.Abstractions;
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
        public async Task<IReadOnlyList<CompanyDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var companies = await _companyRepository.GetAllAsync(cancellationToken);

            return _mapper.Map<List<CompanyDto>>(companies);
        }

        public async Task<CompanyDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var company = await _companyRepository.GetByIdAsync(id, cancellationToken);

            if (company is null)
                return null;

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

        public async Task<bool> UpdateAsync(Guid id, UpdateCompanyDto dto, CancellationToken cancellationToken = default)
        {
            var company = await _companyRepository.GetByIdAsync(id, cancellationToken);

            if (company is null)
                return false;

            _mapper.Map(dto, company);
            company.UpdatedAtUtc = DateTime.UtcNow;

            _companyRepository.Update(company);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var company = await _companyRepository.GetByIdAsync(id, cancellationToken);

            if (company is null)
                return false;

            _companyRepository.Delete(company);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }

    }
}
