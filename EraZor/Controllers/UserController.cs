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

        // GET: api/User/5
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == id);
            if (user == null)
            {
                return NotFound(); // Returner 404, hvis bruger ikke findes
            }
            return Ok(user); // Returner data som JSON
        }

        // POST: api/User
        [HttpPost]
        public IActionResult CreateUser([FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return CreatedAtAction(nameof(GetUserById), new { id = user.UserId }, user);
            }
            return BadRequest(ModelState); // Returner 400, hvis data er ugyldige
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User user)
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

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
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
