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
    public class TransactionRepository: GenericRepository<Transaction>, ITransactionRepository
    {
        private readonly ApplicationDbContext dbContext;

        public TransactionRepository(ApplicationDbContext dbContext): base(dbContext) 
        {
            this.dbContext = dbContext;
        }

        public IQueryable<Transaction> GetQuery()
        {
            return dbContext.Transactions.AsQueryable();
        }

        //public async Task<IEnumerable<Transaction>> GetUserTransactionsAsync(int userId, CancellationToken cancellationToken = default)
        //{
        //    var transactions = await dbContext.Transactions.AsNoTracking().Where(x => x.UserId == userId).ToListAsync(cancellationToken);
        //    return transactions;
        //}
    }
}
