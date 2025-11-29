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


}
