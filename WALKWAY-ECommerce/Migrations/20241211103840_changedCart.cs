using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WALKWAY_ECommerce.Migrations
{
    /// <inheritdoc />
    public partial class changedCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CarTotal",
                table: "Carts",
                newName: "CartTotal");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CartTotal",
                table: "Carts",
                newName: "CarTotal");
        }
    }
}
