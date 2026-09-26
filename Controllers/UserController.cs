using server.Models;
using server.DTOs;
using server.Data;
using server.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;


namespace server.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsersAsync();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserAsync(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserRequest request)
        {

            var user = await _userService.CreateUserAsync(request);

            return CreatedAtAction(
                nameof(GetUser),
                new { id = user.Id },
                user
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id,
        UpdateUserRequest request)
        {
            var user = await _userService.UpdateUserAsync(id, request);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            bool user = await _userService.DeleteUserAsync(id);

            return user ? NoContent() : NotFound();
        }
    }
}
