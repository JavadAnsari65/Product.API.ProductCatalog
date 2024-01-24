using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Product.API.ProductCatalog.Migrations
{
    /// <inheritdoc />
    public partial class Add_ProductCatId_Field : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductCatId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductCatId",
                table: "Products");
        }
    }
}
