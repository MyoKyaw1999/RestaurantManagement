using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagementBackend.Models
{
    public class UserDbContext:DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        [Table("users")]
        public class User
        {
            [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int userId { get; set; }
            public string emailId { get; set; }
            public string password { get; set; }
            public DateTime createdDate { get; set; }

            public string fullName { get; set; }
            public string mobileNo { get; set; }
        }
        [Table("MenuItems")]
        public class MenuItem
        {
            [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public string ItemId { get; set; }
            public string CategoryID { get; set; }
            public string ItemName { get; set; }

            public string Description { get; set; }
            public decimal Price { get; set; }
            public bool Available { get; set; }
            public byte[] Image { get; set; }
            public DateTime CreatedDate { get; set; }
        }
    }
}
