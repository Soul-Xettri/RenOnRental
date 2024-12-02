using Microsoft.AspNetCore.Mvc;
using RevOnRental.Application.Services;
using RevOnRental.Application.Dtos;
using RevOnRental.Application.Interfaces;
using RevOnRental.Domain.Models;
using RevOnRental.Application.Services.Users.Command;
using RevOnRental.Application.Dtos.Auth;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using RevOnRental.Application.Services.Businesses.Command;
using RevOnRental.SignalR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using RevOnRental.Application.Services.Users.Queries;

namespace RevOnRental.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;
        private readonly IHubContext<MessageHub> _messageHub;
        public AuthController(IAuthService authService, IHubContext<MessageHub> messageHub)//, UpdateUser updateUser)
        {
            _authService = authService;
            _messageHub = messageHub;
            //_updateUser = updateUser;
        }

        [AllowAnonymous]
        // Register User
        [HttpPost("register/user")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.RegisterUserAsync(model);
            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest(result.Errors);
        }

        [AllowAnonymous]
        // Register Business
        [HttpPost("register/business")]
        public async Task<IActionResult> RegisterBusiness([FromForm] RegisterBusinessDto model)
        {
            var result = await _authService.RegisterBusinessAsync(model);
           
            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest(result.Errors);
        }
        // Register User
        [HttpPost("add/role")]
        public async Task<IActionResult> AddRole([FromBody] RoleDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authService.CreateRole(model);
            
            return Ok(result);
        }

        // Register User
        [HttpGet("getAllRole")]
        public async Task<IActionResult> GetAllRole()
        {
            
            var result = await _authService.GelAllRole();

            return Ok(result);
        }


        [AllowAnonymous]
        // Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            try
            {
                var result = await _authService.LoginAsync(model);

                //var returnSignalObject = new
                //{
                //    update="signalr works"
                //};
                //await _messageHub.Clients.All.SendAsync("NotificationSend", returnSignalObject);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }

        [HttpGet("user-details/{userId}")]
        public async Task<ActionResult<UserDto>> GetUserDetails(int userId)
        {
            var userDetails = await Mediator.Send(new GetUserDetailsQuery { UserId = userId });
            return Ok(userDetails);
        }
        //updateUser
        [HttpPut("update-user-details/{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] UpdateUserDto updateUserDto)
        {
            try
            {
                var isUpdated = await _authService.UpdateUserAsync(userId, updateUserDto);
                if (isUpdated)
                {
                    return Ok("User information updated successfully.");
                }

                return BadRequest("Failed to update user information.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }

}
