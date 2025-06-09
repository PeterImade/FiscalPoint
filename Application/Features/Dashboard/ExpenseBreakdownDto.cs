using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Dashboard
{
    public class ExpenseBreakdownDto
    {
        public string Category { get; set; }
        public decimal Total { get; set; }
    }
}
