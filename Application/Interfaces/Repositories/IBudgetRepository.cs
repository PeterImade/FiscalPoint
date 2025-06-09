using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IBudgetRepository: IGenericRepository<Budget>
    {
        Task<IEnumerable<Budget>> GetUserBudgetsAsync(int userId, CancellationToken cancellationToken = default);
        IQueryable<Budget> GetQuery();
    }
}
