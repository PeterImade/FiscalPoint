using Application.Features.Transactions;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace Application.Mappings
{
    public static class TransactionMapper
    {
        public static Transaction ToEntity(this CreateTransactionDto createTransactionDto, int userId)
        {
            return new Transaction
            {
                UserId = userId,
                Title = createTransactionDto.Title,
                Category = createTransactionDto.Category,
                Amount = createTransactionDto.Amount,
                Date = createTransactionDto.Date,
                Note = createTransactionDto.Note,
                Type = createTransactionDto.Type
            };
        }

        public static TransactionResponseDto ToDto(this Transaction transaction)
        {
            return new TransactionResponseDto
            {
                Title = transaction.Title,
                Category = transaction.Category,
                Amount = transaction.Amount,
                Date = transaction.Date,
                Type = transaction.Type,
                Note = transaction.Note,
                Id = transaction.Id,
                UserId = transaction.UserId,
                CreatedAt = transaction.CreatedAt,
                UpdatedAt = transaction.UpdatedAt
            };
        }
    }
}
