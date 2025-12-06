using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Emit;

namespace RestaurantManagementBackend.Models
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        [Table("users")]
        public class User
        {
            [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int userId { get; set; }
            public string emailId { get; set; }
            public string userName { get; set; }
            public string passwordHash { get; set; }
            public string fullName { get; set; }
            public string mobileNo { get; set; }
            public string role { get; set; }
            public DateTime createdDate { get; set; }
            public bool isActive { get; set; } = true;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var adminHash = BCrypt.Net.BCrypt.HashPassword("Admin@123");
            var managerHash = BCrypt.Net.BCrypt.HashPassword("Manager@123");
            var casherHash = BCrypt.Net.BCrypt.HashPassword("Casher@123");
            modelBuilder.Entity<MenuItem>()
    .Property(m => m.Price)
    .HasColumnType("decimal(18,2)");


            modelBuilder.Entity<User>().HasData(
                new User
                {
                    userId = 1,
                    emailId = "admin@example.com",
                    passwordHash = adminHash,
                    fullName = "System Admin",
                    mobileNo = "0912345678",
                    userName ="Admin",
                    role = "Admin",
                    createdDate = DateTime.Now,
                    isActive = true
                },
                new User
                {
                    userId = 2,
                    emailId = "manager@example.com",
                    passwordHash = managerHash,
                    fullName = "Restaurant Manager",
                    mobileNo = "0987654321",
                    userName = "Manager",
                    role = "Manager",
                    createdDate = DateTime.Now,
                    isActive = true
                },
                new User
                {
                    userId = 3,
                    emailId = "casher@example.com",
                    passwordHash = casherHash,
                    fullName = "Main Casher",
                    mobileNo = "09911223344",
                    userName = "Casher",
                    role = "Casher",
                    createdDate = DateTime.Now,
                    isActive = true
                }
            );
        }

    }
}
