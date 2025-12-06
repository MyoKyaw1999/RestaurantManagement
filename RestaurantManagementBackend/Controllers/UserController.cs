using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RestaurantManagementBackend.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static RestaurantManagementBackend.Models.UserDbContext;
using BCrypt.Net;

namespace RestaurantManagementBackend.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowCors")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserDbContext _context;
        private readonly IConfiguration _config;

        public UserController(ILogger<UserController> logger, UserDbContext context, IConfiguration config)
        {
            _logger = logger;
            _context = context;
            _config = config;
        }

        // ➤ Create New User
        [HttpPost("CreateNewUser")]
        public IActionResult CreateUser([FromBody] User newUser)
        {
            try
            {
                bool exists = _context.Users.Any(u => u.emailId == newUser.userName);
                if (exists)
                    return BadRequest("User with this email already exists");

                newUser.passwordHash = BCrypt.Net.BCrypt.HashPassword(newUser.passwordHash);
                newUser.createdDate = DateTime.Now;

                _context.Users.Add(newUser);
                _context.SaveChanges();

                return Ok(new { message = "User created", newUser.userId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return StatusCode(500, "Internal server error");
            }
        }

        // ➤ Login and Generate JWT
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.userName == dto.userName);

                if (user == null)
                    return Unauthorized("Invalid userName or password");

                bool isValid = BCrypt.Net.BCrypt.Verify(dto.password, user.passwordHash);

                if (!isValid)
                    return Unauthorized("Invalid userName or password");

                string token = GenerateJwtToken(user);

                return Ok(new
                {
                    message = "Login successful",
                    token,
                    user = new
                    {
                        user.userId,
                        user.fullName,
                        user.role,
                        user.emailId
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login error");
                return StatusCode(500, "Internal server error");
            }
        }

        // ➤ Get All Users
        [HttpGet("getUsers")]
        public IActionResult GetUsers()
        {
            return Ok(_context.Users.ToList());
        }

        // ➤ JWT Generator
        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.Email, user.emailId),
            new Claim(ClaimTypes.Name, user.fullName),
            new Claim(ClaimTypes.Role, user.role)
        };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(12),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        [HttpDelete("DeleteUser/{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.userId == id);
            if (user == null) return NotFound("User not found");

            _context.Users.Remove(user);
            _context.SaveChanges();
            return Ok(new { message = "User deleted successfully" });
        }
        [HttpPut("UpdateUser/{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User updatedUser)
        {
            var user = _context.Users.FirstOrDefault(u => u.userId == id);
            if (user == null) return NotFound("User not found");

            user.fullName = updatedUser.fullName;
            user.userName = updatedUser.userName;
            user.emailId = updatedUser.emailId;
            user.mobileNo = updatedUser.mobileNo;
            user.role = updatedUser.role;
            user.isActive = updatedUser.isActive;

            _context.SaveChanges();
            return Ok(new { message = "User updated successfully" });
        }

    }

    //[Route("api/[controller]")]
    //[ApiController]
    //[EnableCors("AllowCors")]
    //public class UserController : ControllerBase
    //{
    //    private readonly ILogger<UserController> _logger;
    //    private readonly UserDbContext _context;
    //    public UserController(ILogger<UserController> logger, UserDbContext context)
    //    {
    //        _logger = logger;
    //        _context = context;
    //    }
    //    [HttpPost("CreateNewUser")]
    //    public IActionResult CreateUser([FromBody] UserDbContext.User newUser)
    //    {
    //        try
    //        {
    //            var userExistWithEmail = _context.Users.Any(u => u.emailId == newUser.emailId);
    //            if (!userExistWithEmail)
    //            {
    //                newUser.createdDate = DateTime.Now;
    //                _context.Users.Add(newUser);
    //                _context.SaveChanges();
    //                return Ok(new { message = "User created successfully", userId = newUser.userId });
    //            }
    //            else
    //            {
    //                return StatusCode(500, "User with this email already exists");
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError(ex, "Error creating new user");
    //            return StatusCode(500, "Internal server error");
    //        }
    //    }
    //    [HttpPost("Login")]
    //    public IActionResult Login([FromBody] UserDbContext.User loginUser)
    //    {
    //        try
    //        {
    //            var user = _context.Users.FirstOrDefault(u => u.emailId == loginUser.emailId && u.password == loginUser.password);
    //            if (user != null)
    //            {
    //                return Ok(new { message = "Login successful", userId = user.userId });
    //            }
    //            else
    //            {
    //                return StatusCode(401,"Invalid email or password");
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError(ex, "Error during login");
    //            return StatusCode(500, "Internal server error");
    //        }
    //    }
    //    [HttpGet("getUsers")]
    //    public IActionResult GetUsers()
    //    {
    //        try
    //        {
    //            var users = _context.Users.ToList();
    //            return Ok(users);
    //        }
    //        catch (Exception ex)
    //        {
    //            _logger.LogError(ex, "Error fetching users");
    //            return StatusCode(500, "Internal server error");
    //        }
    //    }
    //    public string GenerateJwtToken(User user)
    //    {
    //        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
    //        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

    //        var claims = new[]
    //        {
    //    new Claim(ClaimTypes.Name, user.Username),
    //    new Claim(ClaimTypes.Role, user.Role),
    //    new Claim("FullName", user.FullName)
    //};

    //        var token = new JwtSecurityToken(
    //            issuer: _config["Jwt:Issuer"],
    //            audience: _config["Jwt:Audience"],
    //            claims: claims,
    //            expires: DateTime.Now.AddHours(12),
    //            signingCredentials: credentials
    //        );

    //        return new JwtSecurityTokenHandler().WriteToken(token);
    //    }
    //}
}
