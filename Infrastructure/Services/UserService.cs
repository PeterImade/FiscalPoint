using Application.Exceptions;
using Application.Features.User;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Mappings;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly IUserRepository _userRepository;
        private User? _user;
        public UserService(UserManager<User> userManager, IConfiguration configuration, ICurrentUserProvider currentUserProvider, IUserRepository userRepository)
        {
            _userManager = userManager;
            _configuration = configuration;
            _currentUserProvider = currentUserProvider;
            _userRepository = userRepository;
        }

        public async Task<RegisterResponseDto> RegisterAsync(RegisterUserDto registerUserDto)
        {
            var user = UserMapper.ToEntity(registerUserDto);

            var result = await _userManager.CreateAsync(user, registerUserDto.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description); 
                return new RegisterResponseDto { Errors = errors, IsSuccessful = false };
            }

          
            return new RegisterResponseDto { IsSuccessful = true };
        }

        public async Task<bool> LoginAsync(LoginUserDto loginUserDto)
        {
            _user = await _userManager.FindByEmailAsync(loginUserDto.Email!);

            var result = _user != null && await _userManager.CheckPasswordAsync(_user, loginUserDto.Password!);

            if(!result)
                return false;

            return result;
        }

        public async Task<TokenDto> CreateToken(bool populateExp)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            var refreshToken = GenerateRefreshToken();

            _user.RefreshToken = refreshToken;

            if (populateExp)
                _user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

            await _userManager.UpdateAsync(_user);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return new TokenDto { AccessToken = accessToken, RefreshToken = refreshToken }; 
        }

        private SigningCredentials GetSigningCredentials()
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["secretKey"];
            var key = Encoding.UTF8.GetBytes(secretKey);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, _user.Email),
                new Claim(ClaimTypes.NameIdentifier, _user.Id.ToString())
            };
            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var tokenOptions = new JwtSecurityToken
            (
                issuer: jwtSettings["validIssuer"],
                audience: jwtSettings["validAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expires"])),
                signingCredentials: signingCredentials
            );
            return tokenOptions;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var secretKey = jwtSettings["secretKey"];

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ValidateLifetime = true,
                ValidIssuer = jwtSettings["validIssuer"],
                ValidAudience = jwtSettings["validAudience"]
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out
           securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null ||
           !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
            StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }

        public async Task<TokenDto> RefreshToken(TokenDto tokenDto)
        {
            var principal = GetPrincipalFromExpiredToken(tokenDto.AccessToken);
            var user = await _userManager.FindByNameAsync(principal.Identity.Name);
            if (user == null || user.RefreshToken != tokenDto.RefreshToken ||
            user.RefreshTokenExpiryTime <= DateTime.Now)
                throw new RefreshTokenBadRequest();
            _user = user;
            return await CreateToken(populateExp: false);
        }
         

        public async Task Logout()
        {
            var user = await _currentUserProvider.GetUserAsync();

            if (user is null)
                throw new UnauthorizedAccess("User not authenticated.");

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;

            await _userRepository.UpdateAsync(user);
        }

        public async Task ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var user = await _currentUserProvider.GetUserAsync() ?? throw new UnauthorizedAccess("User not authenticated.");

            var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.OldPassword, changePasswordDto.NewPassword);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description);
                throw new ValidationException($"Password change failed: {string.Join("; ", errors)}");
            }
        }

        public async Task<User?> GetUser()
        {
            var user = await _currentUserProvider.GetUserAsync();

            if (user is null)
                throw new UnauthorizedAccess("User not authenticated.");

            return user;
        }

        public async Task<UserProfileDto?> GetUserProfile()
        {
            var user = await _currentUserProvider.GetUserAsync() ?? throw new UnauthorizedAccess("User not authenticated.");

            var userProfile = new UserProfileDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Currency = user.Currency, 
                UserName = user.UserName
            };

            return userProfile;
        }

        public async Task<UserProfileDto?> UpdateUserProfile(UpdateProfileDto updateProfileDto)
        {
            var user = await _currentUserProvider.GetUserAsync() ?? throw new UnauthorizedAccess("User not authenticated.");

            user.Email = updateProfileDto.Email;
            user.FirstName = updateProfileDto.FirstName;
            user.LastName = updateProfileDto.LastName;
            user.Currency = updateProfileDto.Currency;

            var updatedUser = await _userRepository.UpdateAsync(user);

            var result = new UserProfileDto
            {
                Email = updatedUser.Email,
                FirstName = updatedUser.FirstName,
                LastName = updatedUser.LastName,
                Currency = updatedUser.Currency
            };

            return result;
        }
    }
}
