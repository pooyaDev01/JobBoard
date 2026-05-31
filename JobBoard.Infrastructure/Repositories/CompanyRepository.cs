using JobBoard.Application.Abstractions;
using JobBoard.Domain.Entities;
using JobBoard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Infrastructure.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly JobBoardDbContext _context;

        public CompanyRepository(JobBoardDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Company>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Companies
               .AsNoTracking()
               .ToListAsync(cancellationToken);
        }

        public async Task<Company?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Companies
              .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task AddAsync(Company company, CancellationToken cancellationToken = default)
        {
            await _context.Companies.AddAsync(company, cancellationToken);
        }

        public void Update(Company company)
        {
            _context.Companies.Update(company);
        }

        public void Delete(Company company)
        {
            _context.Companies.Remove(company);
        }

    }
}
