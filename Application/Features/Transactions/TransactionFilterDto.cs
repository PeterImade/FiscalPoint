using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Features.Transactions
{
    public class TransactionFilterDto
    {
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
        public string Category { get; set; } = string.Empty;
        
        [JsonPropertyName("Type")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TransactionType? Type { get; set; }
    }
}
