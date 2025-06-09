using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Dashboard
{
    public class CategoryBreakdownDto
    {
        public string Category { get; set; } = default!;
        public decimal TotalAmount { get; set; }
    }
}
