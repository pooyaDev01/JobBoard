using JobBoard.Application.Abstractions;
using JobBoard.Domain.Entities;
using JobBoard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Infrastructure.Repositories
{
    public class JobRepository : IJobRepository
    {
        private readonly JobBoardDbContext _context;

        public JobRepository(JobBoardDbContext context)
        {
            _context = context;
        }


        public async Task AddAsync(Job job, CancellationToken cancellationToken = default)
        {
            await _context.Jobs.AddAsync(job, cancellationToken);
        }

        public void Delete(Job job)
        {
            _context.Jobs.Remove(job);
        }

        public async Task<IReadOnlyList<Job>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Jobs
                .AsNoTracking()
                .ToListAsync(cancellationToken);   
        }

        public async Task<Job?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Jobs
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public void Update(Job job)
        {
            _context.Jobs.Update(job);
        }
    }
}
