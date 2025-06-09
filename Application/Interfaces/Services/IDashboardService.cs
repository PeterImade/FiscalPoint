using Application.Features.Dashboard;
using Application.Features.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IDashboardService
    {
        Task<DashboardSummaryDto> GetDashboardSummary(CancellationToken cancellationToken = default); 
        Task<List<IncomeVsExpenseDto>> GetIncomeVsExpenseTrendAsync(CancellationToken cancellationToken = default);
        Task<List<CategoryBreakdownDto>> GetCategoryBreakdownAsync(CancellationToken cancellationToken = default);
        Task<List<TransactionResponseDto>?> GetRecentTransactions(CancellationToken cancellationToken = default);
        Task<SavingsRateDto> GetSavingsRateAsync(CancellationToken cancellationToken = default);
    }
}
