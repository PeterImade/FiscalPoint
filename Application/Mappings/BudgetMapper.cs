using Application.Features.Budgets;
using Application.Features.Transactions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings
{
    public static class BudgetMapper
    {
        public static Budget ToEntity(this CreateBudgetDto createBudgetDto, int userId)
        {
            return new Budget
            {
                UserId = userId,
                Amount = createBudgetDto.Amount,
                Category = createBudgetDto.Category,
                Month = createBudgetDto.Month,
                Year = createBudgetDto.Year
            };
        }

        public static BudgetResponseDto ToDto(this Budget budget)
        {
            return new BudgetResponseDto
            {
                 Id = budget.Id,
                 Month = budget.Month,
                 Category = budget.Category,
                 Amount = budget.Amount,
                 CreatedAt = budget.CreatedAt,
                 UpdatedAt = budget.UpdatedAt,
                 UserId = budget.UserId,
                 Year = budget.Year
            };
        }
    }
}
