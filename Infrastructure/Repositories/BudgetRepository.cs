using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class BudgetRepository: GenericRepository<Budget>, IBudgetRepository
    {
        private readonly ApplicationDbContext dbContext;

        public BudgetRepository(ApplicationDbContext dbContext): base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public IQueryable<Budget> GetQuery()
        {
            return dbContext.Budgets.AsQueryable();
        }

        public async Task<IEnumerable<Budget>> GetUserBudgetsAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await dbContext.Budgets.AsNoTracking().Where(x => x.UserId == userId).ToListAsync(cancellationToken);
        }
    }
}