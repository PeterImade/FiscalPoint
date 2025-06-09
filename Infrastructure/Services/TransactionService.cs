using Application.Exceptions;
using Application.Features.Transactions;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Mappings;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Infrastructure.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly ILogger<TransactionService> _logger;

        public TransactionService(ITransactionRepository transactionRepository, ICurrentUserProvider currentUserProvider, ILogger<TransactionService> logger)
        {
            _transactionRepository = transactionRepository;
            _currentUserProvider = currentUserProvider;
            _logger = logger;
        }
        public async Task<TransactionResponseDto> CreateAsync(CreateTransactionDto createTransactionDto, CancellationToken cancellationToken = default)
        {
            var userId = await _currentUserProvider.GetUserIdAsync(cancellationToken) ?? throw new UnauthorizedAccess("User not authenticated.");

            _logger.LogInformation("Creating transaction...");
           
            var entity = TransactionMapper.ToEntity(createTransactionDto, userId);
            var createdTransaction = await _transactionRepository.CreateAsync(entity, cancellationToken);

            _logger.LogInformation("Successfully created transaction.");

            return TransactionMapper.ToDto(createdTransaction);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var userId = await _currentUserProvider.GetUserIdAsync(cancellationToken) ?? throw new UnauthorizedAccess("User not authenticated.");

            _logger.LogInformation("Fetching transaction...");
            
            var transactionToDelete = await _transactionRepository.GetAsync(id, cancellationToken);

            if (transactionToDelete is null)
                throw new NotFoundException($"Transaction with the id: {id} not found!");

            if (transactionToDelete?.UserId != userId)
                throw new UnauthorizedAccess("You are not authorized to access this route.");

            await  _transactionRepository.DeleteAsync(transactionToDelete, cancellationToken);

            _logger.LogInformation("Transaction deleted successfully...");
        }

        public async Task<TransactionResponseDto?> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            var userId = await _currentUserProvider.GetUserIdAsync(cancellationToken) ?? throw new UnauthorizedAccess("User not authenticated.");

            _logger.LogInformation("Fetching transaction...");

            var transaction = await _transactionRepository.GetAsync(id, cancellationToken) ?? throw new NotFoundException($"Transaction with the id: {id} not found!");

            if (transaction?.UserId != userId)
                throw new UnauthorizedAccess("You are not authorized to access this route.");

            _logger.LogInformation("Transaction retrieved successfully...");
            return TransactionMapper.ToDto(transaction);
        }

        public async Task<TransactionResponseDto?> UpdateAsync(int id, CreateTransactionDto createTransactionDto, CancellationToken cancellationToken = default)
        {
            var userId = await _currentUserProvider.GetUserIdAsync() ?? throw new UnauthorizedAccess("User not authenticated.");

            var transaction = await _transactionRepository.GetAsync(id, cancellationToken);

            if (transaction is null)
                throw new NotFoundException($"Transaction with the id: {id} not found!");

            if (transaction?.UserId != userId)
                throw new UnauthorizedAccess("You are not authorized to access this route.");

            var transactionToUpdate = TransactionMapper.ToEntity(createTransactionDto, userId);

            transactionToUpdate.UpdatedAt = DateTime.Now;

            _logger.LogInformation("Updating transaction...");

            var updatedTransaction = await _transactionRepository.UpdateAsync(transactionToUpdate, cancellationToken);

            _logger.LogInformation("Transaction updated successfully!");

            return TransactionMapper.ToDto(updatedTransaction);
        }

        public async Task<PagedResult<TransactionResponseDto>?> GetPagedAndFilteredTransactions(TransactionFilterDto filterDto, CancellationToken cancellationToken = default)
        {
            var userId = await _currentUserProvider.GetUserIdAsync() ?? throw new UnauthorizedAccess("User not authenticated.");

            var query = _transactionRepository.GetQuery().Where(x => x.UserId == userId);

            if (!string.IsNullOrEmpty(filterDto.Category))
            {
                query = query.Where(x => x.Category.ToLower() ==  filterDto.Category.ToLower());
            }

            if (filterDto.Type is not null)
            {
                query = query.Where(x => x.Type == filterDto.Type);
            }

            var totalCount = await query.CountAsync(cancellationToken); 

            var transactions = await query
               .OrderBy(x => x.Date) 
               .Select(x => TransactionMapper.ToDto(x))
               .ToPagedListAsync(filterDto.PageSize, filterDto.PageNumber);

            return transactions;
        }
    }
}