using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.X509;
using RestaurantManagementBackend.Models;
using static RestaurantManagementBackend.Models.UserDbContext;

namespace RestaurantManagementBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowCors")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;
        private readonly ILogger<OrderController> _logger;
        private readonly UserDbContext _context;
        public OrderController(ILogger<OrderController> logger, UserDbContext context, OrderService orderService)
        {
            _logger = logger;
            _context = context;
            _orderService = orderService;
        }
        public ActionResult<List<Order>> GetOrders()
        {
            return Ok(_orderService.GetOrders());
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
        public ActionResult<Order> SaveOrder([FromBody] Order order)
        {
            if (order.Items == null || order.Items.Count == 0)
            {
                return BadRequest("Order must have at least one item.");
            }

            order.CreatedAt = DateTime.UtcNow;
            var savedOrder = _orderService.SaveOrder(order);
            return Ok(savedOrder);
        }
    }
}
