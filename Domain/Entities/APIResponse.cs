using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class APIResponse
    {
        public bool Success { get; set; } = true;
        public string? Message { get; set; } = null;
        public object? Data { get; set; }
    }
}
