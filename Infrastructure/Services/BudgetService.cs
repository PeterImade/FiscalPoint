
using Application.Exceptions;
using Application.Features.Budgets;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Mappings;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class BudgetService : IBudgetService
    {
        private readonly IBudgetRepository _budgetRepository;
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly ILogger<BudgetService> _logger;

        public BudgetService(IBudgetRepository budgetRepository, ICurrentUserProvider currentUserProvider, ILogger<BudgetService> logger)
        {
            _budgetRepository = budgetRepository;
            _currentUserProvider = currentUserProvider;
            _logger = logger;
        }
        public async Task<BudgetResponseDto> CreateAsync(CreateBudgetDto createBudgetDto, CancellationToken cancellationToken = default)
        {
            var userId = await _currentUserProvider.GetUserIdAsync();

            _logger.LogInformation("Creating budget...");

            if (userId is null)
            {
                _logger.LogCritical("User is not authenticated!");
                throw new UnauthorizedAccess("User not authenticated.");
            }

            var budget = BudgetMapper.ToEntity(createBudgetDto, userId.Value);
            var createdBudget = await _budgetRepository.CreateAsync(budget, cancellationToken);

            _logger.LogInformation("Budget created successfully.");

            return BudgetMapper.ToDto(createdBudget);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var userId = await _currentUserProvider.GetUserIdAsync();

            if (userId is null)
            {
                _logger.LogError("User is not authenticated!");
                throw new UnauthorizedAccess("User not authenticated.");
            }

            _logger.LogInformation("Fetching budget to be deleted...");

            var budgetToDelete = await _budgetRepository.GetAsync(id, cancellationToken);

            if (budgetToDelete is null)
            {
                _logger.LogError("Couldn't find budget!");
                throw new NotFoundException($"Budget with the id: {id} not found!");
            }

            if (budgetToDelete?.UserId != userId.Value)
            {
                _logger.LogError("User is not authorized to access this route!");

                throw new UnauthorizedAccess("You are not authorized to access this route.");
            }

            await _budgetRepository.DeleteAsync(budgetToDelete, cancellationToken);

            _logger.LogInformation("Budget deleted successfully.");
        }

        public async Task<PagedResult<BudgetResponseDto>?> GetAllAsync(BudgetFilterDto budgetFilterDto, CancellationToken cancellationToken = default)
        {
            var userId = await _currentUserProvider.GetUserIdAsync() ?? throw new UnauthorizedAccess("User not authenticated.");

            var query = _budgetRepository.GetQuery().Where(x => x.UserId == userId);

            var totalCount = await query.CountAsync(cancellationToken);

            var budgets = await query
              .Select(x => BudgetMapper.ToDto(x)).ToPagedListAsync(budgetFilterDto.PageSize, budgetFilterDto.PageNumber);

            return budgets;
        }

        public async Task<BudgetResponseDto?> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            var userId = await _currentUserProvider.GetUserIdAsync();

            if (userId is null)
                throw new UnauthorizedAccess("User not authenticated.");

            var budget = await _budgetRepository.GetAsync(id, cancellationToken);

            if(budget is null)
                throw new NotFoundException($"Budget with the id: {id} not found!");

            if (budget?.UserId != userId.Value)
                throw new UnauthorizedAccess("You are not authorized to access this route.");

            return BudgetMapper.ToDto(budget);
        }

        public async Task<BudgetResponseDto?> UpdateAsync(int id, CreateBudgetDto updateBudgetDto, CancellationToken cancellationToken = default)
        {
            var userId = await _currentUserProvider.GetUserIdAsync();

            if (userId is null)
                throw new UnauthorizedAccess("User not authenticated.");

            var budget = await _budgetRepository.GetAsync(id, cancellationToken);

            if (budget is null)
                throw new NotFoundException($"Budget with the id: {id} not found!");

            if (budget?.UserId != userId.Value)
                throw new UnauthorizedAccess("You are not authorized to access this route.");

            var budgetToUpdate = BudgetMapper.ToEntity(updateBudgetDto, userId.Value);

            budgetToUpdate.UpdatedAt = DateTime.Now;

            _logger.LogInformation("Budget updating...");

            var updatedBudget = await _budgetRepository.UpdateAsync(budgetToUpdate, cancellationToken);
            _logger.LogInformation("Budget updated successfully.");

            return BudgetMapper.ToDto(updatedBudget);
        }
    }
}