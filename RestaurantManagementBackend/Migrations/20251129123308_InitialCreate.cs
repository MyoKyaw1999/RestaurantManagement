using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RestaurantManagementBackend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MenuItems",
                columns: table => new
                {
                    ItemId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CategoryID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Available = table.Column<bool>(type: "bit", nullable: false),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItems", x => x.ItemId);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    userId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    emailId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    userName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    passwordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    fullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    mobileNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    createdDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.userId);
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "userId", "createdDate", "emailId", "fullName", "isActive", "mobileNo", "passwordHash", "role", "userName" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 11, 29, 19, 3, 6, 889, DateTimeKind.Local).AddTicks(8339), "admin@example.com", "System Admin", true, "0912345678", "$2a$11$HpheXDz2T5mtzPYjYxZ83uJKUssgpjch3Qr94uKT2iznROkflXgOO", "Admin", "Admin" },
                    { 2, new DateTime(2025, 11, 29, 19, 3, 6, 889, DateTimeKind.Local).AddTicks(8355), "manager@example.com", "Restaurant Manager", true, "0987654321", "$2a$11$5gapJTxVbsDMWDbKlvFvdeashZeGobP5wzVU/xURWdC1Aj4noub8u", "Manager", "Manager" },
                    { 3, new DateTime(2025, 11, 29, 19, 3, 6, 889, DateTimeKind.Local).AddTicks(8356), "casher@example.com", "Main Casher", true, "09911223344", "$2a$11$OBmUZz95Vbnt59Mw/4yZf.jRGnRNUr6ENTfXNPlMz19BwySu0Xa4q", "Casher", "Casher" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MenuItems");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
