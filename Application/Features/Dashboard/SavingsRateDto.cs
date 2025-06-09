using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Dashboard
{
    public class SavingsRateDto
    { 
            public string Month { get; set; } = string.Empty;
            public decimal TotalIncome { get; set; }
            public decimal TotalExpense { get; set; }
            public decimal AmountSaved { get; set; }
            public decimal SavingsRate { get; set; }
        }
}
