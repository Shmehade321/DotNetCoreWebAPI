using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DotNetCoreWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Shirts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Size = table.Column<int>(type: "int", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shirts", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Shirts",
                columns: new[] { "Id", "Brand", "Color", "Gender", "Price", "Size" },
                values: new object[,]
                {
                    { 1, "Nike", "Black", "men", 29.989999999999998, 7 },
                    { 2, "Addidas", "Yellow", "women", 59.990000000000002, 10 },
                    { 3, "Atletics", "Brown", "men", 19.989999999999998, 9 },
                    { 4, "Puma", "White", "women", 39.990000000000002, 12 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Shirts");
        }
    }
}
