using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Budgets
{
    public class BudgetFilterDto
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string Category { get; set; } = string.Empty;

        [JsonPropertyName("Type")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TransactionType? Type { get; set; }
    }
}