using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Transactions
{
    public class CreateTransactionDto
    {
        [Required(ErrorMessage = "Title must be required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Category must be required")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Amount must be required")]
        public decimal Amount { get; set; }

        [JsonPropertyName("Type")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [Required(ErrorMessage = "Transaction Type is required")]
        public TransactionType Type { get; set; }
        public DateOnly Date { get; set; }
        public string? Note { get; set; }
    }
}