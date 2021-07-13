using Microsoft.EntityFrameworkCore.Migrations;

namespace Aroma_Shop.Data.Migrations
{
    public partial class AddProductHitsPropertyToProductModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductHits",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductHits",
                table: "Products");
        }
    }
}
