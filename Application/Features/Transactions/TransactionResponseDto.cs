using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Transactions
{
    public class TransactionResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public required string Title { get; set; } 
        public required decimal Amount { get; set; }
        public required string Category { get; set; }
        public TransactionType Type { get; set; }
        public DateOnly Date { get; set; }
        public string? Note { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
