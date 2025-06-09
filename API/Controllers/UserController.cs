using Application.Features.User;
using Application.Interfaces.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpGet("profile")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserProfile()
        {
            var profile = await _userService.GetUserProfile();

            return Ok(new APIResponse { Message = "User's profile retrieved successfully!", Data = profile});
        }
        
        [Authorize]
        [HttpPut("profile")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UpdateProfileDto updateProfileDto)
        {
            var updatedProfile = await _userService.UpdateUserProfile(updateProfileDto);

            return Ok(new APIResponse { Message = "User's profile updated successfully!", Data = updatedProfile });
        }
    }
}