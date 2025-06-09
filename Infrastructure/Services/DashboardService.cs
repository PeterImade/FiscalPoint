using Application.Exceptions;
using Application.Features.Dashboard;
using Application.Features.Transactions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Mappings;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ILogger<DashboardService> _logger;

        public DashboardService(ICurrentUserProvider currentUserProvider, ITransactionRepository transactionRepository, ILogger<DashboardService> logger)
        {
            _currentUserProvider = currentUserProvider;
            _transactionRepository = transactionRepository;
            _logger = logger;
        }

        public async Task<List<CategoryBreakdownDto>> GetCategoryBreakdownAsync(CancellationToken cancellationToken = default)
        {
            var userId = await _currentUserProvider.GetUserIdAsync(cancellationToken) ?? throw new UnauthorizedAccessException("User not authenticated.");

            var transactions = await _transactionRepository
                .GetQuery()
                .Where(x => x.UserId == userId && x.Type == TransactionType.Expense)
                .ToListAsync(cancellationToken);

            var result = transactions
            .GroupBy(t => t.Category)
            .Select(g => new CategoryBreakdownDto
            {
                Category = g.Key,
                TotalAmount = g.Sum(t => t.Amount)
            })
            .OrderByDescending(x => x.TotalAmount)
            .ToList();

            return result;
        }

        public async Task<DashboardSummaryDto> GetDashboardSummary(CancellationToken cancellationToken = default)
        {
            var userId = await _currentUserProvider.GetUserIdAsync(cancellationToken);

            if (userId is null)
                throw new UnauthorizedAccess("User not authenticated.");

            var transactions = await _transactionRepository.GetQuery().AsNoTracking().Where(x => x.UserId == userId).ToListAsync(cancellationToken);

            var totalIncome = transactions.Where(x => x.Type == TransactionType.Income).Sum(x => x.Amount);

            var totalExpense = transactions.Where(x => x.Type == TransactionType.Expense).Sum(x => x.Amount);

            _logger.LogInformation("Dashboard summary retrieved successfully");

            return new DashboardSummaryDto { TotalExpense = totalExpense, TotalIncome = totalIncome };
        }

        public async Task<List<IncomeVsExpenseDto>> GetIncomeVsExpenseTrendAsync(CancellationToken cancellationToken = default) // Shows monthly comparison of income and expenses.
        {
            var userId = await _currentUserProvider.GetUserIdAsync(cancellationToken) ?? throw new UnauthorizedAccess("User not authenticated.");

            var transactions = await _transactionRepository.GetQuery().Where(x => x.UserId == userId).ToListAsync(cancellationToken);

            var result = transactions
            .GroupBy(t => t.Date.ToString("yyyy-MM"))
            .Select(g => new IncomeVsExpenseDto
            {
                Period = DateTime.ParseExact(g.Key, "yyyy-MM", null).ToString("MMM yyyy"),
                Income = g.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount),
                Expense = g.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount)
            })
            .OrderBy(dto => dto.Period)
            .ToList();

            return result;
        }

        public async Task<List<TransactionResponseDto>?> GetRecentTransactions(CancellationToken cancellationToken = default)
        {
            var userId = await _currentUserProvider.GetUserIdAsync(cancellationToken) ?? throw new UnauthorizedAccess("User not authenticated.");

            var transactions = await _transactionRepository.GetQuery().Where(x => x.UserId == userId).OrderByDescending(x => x.CreatedAt).Take(5).ToListAsync(cancellationToken);

            var recentTransactions = transactions.Select(x => TransactionMapper.ToDto(x)).ToList();

            _logger.LogInformation("Recent transactions retrieved successfully");

            return recentTransactions;
        }

        public async Task<SavingsRateDto> GetSavingsRateAsync(CancellationToken cancellationToken = default)
        {
            var userId = await _currentUserProvider.GetUserIdAsync() ?? throw new UnauthorizedAccess("User not authenticated.");
            return null;
        }
    }
}