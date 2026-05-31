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

        public CompanyService(ICompanyRepository companyRepository, IUnitOfWork unitOfWork)
        {
            _companyRepository = companyRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<IReadOnlyList<CompanyDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var companies = await _companyRepository.GetAllAsync(cancellationToken);

            return companies
                .Select(c => new CompanyDto(
                    c.Id,
                    c.Name,
                    c.Description,
                    c.Website,
                    c.Location,
                    c.CreatedAtUtc,
                    c.UpdatedAtUtc))
                .ToList();
        }

        public async Task<CompanyDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var company = await _companyRepository.GetByIdAsync(id, cancellationToken);

            if (company is null)
                return null;

            return new CompanyDto(
                company.Id,
                company.Name,
                company.Description,
                company.Website,
                company.Location,
                company.CreatedAtUtc,
                company.UpdatedAtUtc);
        }
        public async Task<CompanyDto> CreateAsync(CreateCompanyDto dto, CancellationToken cancellationToken = default)
        {
            var company = new Company
            {
                Id = Guid.NewGuid(),
                Name = dto.Name!,
                Description = dto.Description,
                Website = dto.Website,
                Location = dto.Location,
                CreatedAtUtc = DateTime.UtcNow
            };

            await _companyRepository.AddAsync(company, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CompanyDto(
                company.Id,
                company.Name,
                company.Description,
                company.Website,
                company.Location,
                company.CreatedAtUtc,
                company.UpdatedAtUtc);
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateCompanyDto dto, CancellationToken cancellationToken = default)
        {
            var company = await _companyRepository.GetByIdAsync(id, cancellationToken);

            if (company is null)
                return false;

            company.Name = dto.Name;
            company.Description = dto.Description;
            company.Website = dto.Website;
            company.Location = dto.Location;
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
