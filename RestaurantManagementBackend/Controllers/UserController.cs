using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagementBackend.Models;

namespace RestaurantManagementBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowCors")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserDbContext _context;
        public UserController(ILogger<UserController> logger, UserDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        [HttpPost("CreateNewUser")]
        public IActionResult CreateUser([FromBody] UserDbContext.User newUser)
        {
            try
            {
                var userExistWithEmail = _context.Users.Any(u => u.emailId == newUser.emailId);
                if (!userExistWithEmail)
                {
                    newUser.createdDate = DateTime.Now;
                    _context.Users.Add(newUser);
                    _context.SaveChanges();
                    return Ok(new { message = "User created successfully", userId = newUser.userId });
                }
                else
                {
                    return StatusCode(500, "User with this email already exists");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating new user");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost("Login")]
        public IActionResult Login([FromBody] UserDbContext.User loginUser)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.emailId == loginUser.emailId && u.password == loginUser.password);
                if (user != null)
                {
                    return Ok(new { message = "Login successful", userId = user.userId });
                }
                else
                {
                    return StatusCode(401,"Invalid email or password");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("getUsers")]
        public IActionResult GetUsers()
        {
            try
            {
                var users = _context.Users.ToList();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching users");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
