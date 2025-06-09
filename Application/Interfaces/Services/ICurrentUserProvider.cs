using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface ICurrentUserProvider
    {
        string? UserName { get; }
        Task<User?> GetUserAsync(CancellationToken cancellationToken = default);
        Task<int?> GetUserIdAsync(CancellationToken cancellationToken = default);
    }
}
