using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Budgets
{
    public class CreateBudgetDto
    {

        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; }

        [Precision(18, 2)]
        public decimal Amount { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
