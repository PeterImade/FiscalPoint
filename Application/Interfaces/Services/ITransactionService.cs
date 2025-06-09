using Application.Features.Transactions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface ITransactionService
    {
        Task<TransactionResponseDto> CreateAsync(CreateTransactionDto createTransactionDto, CancellationToken cancellationToken = default);
        Task<TransactionResponseDto?> GetAsync(int id, CancellationToken cancellationToken = default);
        Task<TransactionResponseDto?> UpdateAsync(int id, CreateTransactionDto createTransactionDto, CancellationToken cancellationToken = default);
        Task<PagedResult<TransactionResponseDto>?> GetPagedAndFilteredTransactions(TransactionFilterDto filterDto, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
