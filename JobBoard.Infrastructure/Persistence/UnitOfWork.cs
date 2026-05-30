using JobBoard.Application.Abstractions;
using JobBoard.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly JobBoardDbContext _context;

        public UnitOfWork(JobBoardDbContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
