using server.Models;
using server.DTOs;
using server.Data;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace server.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _db;

        public UsersController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _db.Users.ToListAsync();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            User? user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserRequest request)
        {

            var user = new User
            {
                Name = request.Name,
                Email = request.Email
            };

            _db.Users.Add(user);

            await _db.SaveChangesAsync();

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
            var user = await _db.Users
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            user.Name = request.Name;
            user.Email = request.Email;

            await _db.SaveChangesAsync();

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _db.Users
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            _db.Users.Remove(user);

            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
