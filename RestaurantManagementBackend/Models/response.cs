namespace RestaurantManagementBackend.Models
{
    public class MenuItemDto
    {
        public string ItemId { get; set; }
        public string CategoryID { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool Availible { get; set; }
        public string Image { get; set; } // Base64 string
    }
    public class LoginDto
    {
        public string userName { get; set; }
        public string password { get; set; }
    }
    // Models/OrderItem.cs
    public class OrderItem
    {
        public string ItemId { get; set; } = null!;
        public string ItemName { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
    }

    // Models/Order.cs
    public class Order
    {
        public string OrderId { get; set; } = null!;
        public string OrderType { get; set; } = null!; // "TABLE" or "PICKUP"
        public string? TableNo { get; set; }
        public List<OrderItem> Items { get; set; } = new();
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
