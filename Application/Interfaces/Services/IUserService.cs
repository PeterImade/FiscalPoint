using Application.Features.User;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<RegisterResponseDto> RegisterAsync(RegisterUserDto registerUserDto);
        Task<bool> LoginAsync(LoginUserDto loginUserDto);
        Task<TokenDto> CreateToken(bool populateExp);
        Task<TokenDto> RefreshToken(TokenDto tokenDto);
        Task Logout(); 
        Task ChangePassword(ChangePasswordDto changePasswordDto);
        Task<UserProfileDto?> GetUserProfile();
        Task<UserProfileDto?> UpdateUserProfile(UpdateProfileDto updateProfileDto);
        Task<User?> GetUser(); 
    }
}
