using Application.Features.Budgets;
using Application.Features.Transactions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IBudgetService
    {
        Task<BudgetResponseDto> CreateAsync(CreateBudgetDto createBudgetDto, CancellationToken cancellationToken = default);
        Task<PagedResult<BudgetResponseDto>?> GetAllAsync(BudgetFilterDto budgetFilterDto, CancellationToken cancellationToken = default);
        Task<BudgetResponseDto?> GetAsync(int id, CancellationToken cancellationToken = default);
        Task<BudgetResponseDto?> UpdateAsync(int id, CreateBudgetDto createBudgetDto, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}