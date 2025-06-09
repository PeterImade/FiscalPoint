
using Application.Features.User;
using Application.Interfaces.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto registerUserDto)
        {
            if (registerUserDto is null)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid user data.", Errors = ModelState.Values.SelectMany(v => v.Errors) });
            }

            var response = await _userService.RegisterAsync(registerUserDto);

            if (response.Errors is not null)
            {
                return StatusCode(400, response);
            }
            return StatusCode(201, response);
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserDto loginUserDto)
        {
            if (loginUserDto is null)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid user data.", Errors = ModelState.Values.SelectMany(v => v.Errors) });
            }

            if (!await _userService.LoginAsync(loginUserDto))
            {
                return Unauthorized();
            }

            var tokenDto = await _userService.CreateToken(populateExp: true);

            return Ok(tokenDto);
        }

        [HttpPost("refresh")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> RefreshToken([FromBody] TokenDto tokenDto)
        {
            var accessToken = await _userService.RefreshToken(tokenDto);
            return Ok(accessToken);
        }


        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> LogoutUser()
        {
            await _userService.Logout();

            return Ok(new { Message = "Logout successful!"});
        }

        [HttpPut("change-password")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Invalid user data.", Errors = ModelState.Values.SelectMany(v => v.Errors) });
            }

            await _userService.ChangePassword(changePasswordDto);

            return Ok(new {Message = "Password changed successfully."});
        }

        [HttpGet("me")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCurrentUserInfo()
        {
           var currentUser = await _userService.GetUser();

            return Ok(new APIResponse { Message = "User info. retrieved successfully!",  Data = currentUser});
        }


    }
}
