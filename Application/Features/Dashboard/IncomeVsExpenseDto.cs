using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Dashboard
{
    public class IncomeVsExpenseDto
    {
        public string Period { get; set; }
        public decimal Income { get; set; }
        public decimal Expense { get; set; }
    }
}
