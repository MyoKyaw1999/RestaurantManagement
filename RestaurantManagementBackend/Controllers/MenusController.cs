using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementBackend.Models;
using static RestaurantManagementBackend.Models.UserDbContext;

namespace RestaurantManagementBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowCors")]
    public class MenusController : ControllerBase
    {
        private readonly ILogger<MenusController> _logger;
        private readonly UserDbContext _context;
        public MenusController(ILogger<MenusController> logger, UserDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuItemDto>>> GetMenuItems()
        {
            var data = await _context.MenuItems.ToListAsync();

            var result = data.Select(x => new MenuItemDto
            {
                ItemId = x.ItemId,
                CategoryID = x.CategoryID,
                ItemName = x.ItemName,
                Description = x.Description,
                Price = x.Price,
                Availible = x.Available,
                Image = x.Image != null ?
                    "data:image/png;base64," + Convert.ToBase64String(x.Image)
                    : null
            });

            return Ok(result);
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<MenuItem>> GetMenuItem(string id)
        {
            var item = await _context.MenuItems.FindAsync(id);

            if (item == null)
                return NotFound();

            return item;
        }

        [HttpPost]
        public async Task<ActionResult<MenuItem>> CreateMenuItem(MenuItem model)
        {
            try
            {
                model.CreatedDate = DateTime.Now;
                model.ItemId = Guid.NewGuid().ToString();
                _context.MenuItems.Add(model);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetMenuItem), new { id = model.ItemId }, model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An error occurred while creating the menu item.",
                    Error = ex.Message
                });
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMenuItem(string id, MenuItem model)
        {
            if (id != model.ItemId)
                return BadRequest("ItemId mismatch");

            var existing = await _context.MenuItems.FindAsync(id);
            if (existing == null)
                return NotFound();

            // Update fields
            existing.ItemName = model.ItemName;
            existing.CategoryID = model.CategoryID;
            existing.Description = model.Description;
            existing.Price = model.Price;
            existing.Available = model.Available;

            // Only update image if provided
            if (model.Image != null && model.Image.Length > 0)
            {
                existing.Image = model.Image;
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenuItem(string id)
        {
            var item = await _context.MenuItems.FindAsync(id);
            if (item == null)
                return NotFound();

            _context.MenuItems.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
