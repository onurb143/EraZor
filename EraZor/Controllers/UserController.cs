using Microsoft.AspNetCore.Mvc;
using EraZor.Data; // Til databasen
using EraZor.Models; // Til User-modellen
using System.Linq;

namespace EraZor.Controllers
{
    [ApiController] // Marker controlleren som en API-controller
    [Route("api/[controller]")] // Definer baseruten for denne controller (f.eks. /api/User)
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;

        // Constructor for at injicere DataContext
        public UserController(DataContext context)
        {
            _context = context;
        }

        // GET: api/User
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _context.Users.ToList();
            return Ok(users); // Returner data som JSON
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(string id) // Skift til string
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == id);
            if (user == null)
            {
                return NotFound(); // Returner 404, hvis bruger ikke findes
            }
            return Ok(user); // Returner data som JSON
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(string id, [FromBody] User user) // Skift til string
        {
            if (id != user.UserId)
            {
                return BadRequest("User ID mismatch.");
            }

            if (!_context.Users.Any(u => u.UserId == id))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Users.Update(user);
                _context.SaveChanges();
                return NoContent(); // Returner 204 ved succes
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(string id) // Skift til string
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            _context.SaveChanges();
            return NoContent();
        }

    }
}
