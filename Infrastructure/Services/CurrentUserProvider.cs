using Application.Interfaces.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class CurrentUserProvider : ICurrentUserProvider
    {
        private readonly HttpContext _httpContext;
        private readonly UserManager<User> _userManager;

        public CurrentUserProvider(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
        {
            this._httpContext = httpContextAccessor.HttpContext!;
            this._userManager = userManager;
        }
        public string? UserName => _httpContext.User?.Identity?.Name;

        public async Task<User?> GetUserAsync(CancellationToken cancellationToken = default) => await _userManager.FindByEmailAsync(UserName);

        public async Task<int?> GetUserIdAsync(CancellationToken cancellationToken = default)
        {
            var user = await GetUserAsync(cancellationToken);
            return user?.Id;
        }
    }
}
