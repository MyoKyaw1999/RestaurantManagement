namespace RestaurantManagementBackend.Models
{
    public class OrderService
    {
        private readonly List<Order> _orders = new();

        public List<Order> GetOrders() => _orders;

        public Order SaveOrder(Order order)
        {
            _orders.Add(order);
            return order;
        }
    }
}
