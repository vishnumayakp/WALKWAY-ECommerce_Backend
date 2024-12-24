using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WALKWAY_ECommerce.Migrations
{
    /// <inheritdoc />
    public partial class CartChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductBrand",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductBrand",
                table: "Products");
        }
    }
}
