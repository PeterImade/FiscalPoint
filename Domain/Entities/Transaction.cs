using Domain.Common;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Transaction: BaseEntity<int>
    {
        public int UserId { get; set; }
        public required string Title { get; set; }
        [Precision(18, 2)]
        public required decimal Amount { get; set; }
        public required string Category { get; set; }
        public TransactionType Type { get; set; }
        public DateOnly Date { get; set; }
        public string? Note { get; set; }
    }
}
