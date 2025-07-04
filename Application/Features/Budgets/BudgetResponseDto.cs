﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Budgets
{
    public class BudgetResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Category { get; set; }

        [Precision(18, 2)]
        public decimal Amount { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
