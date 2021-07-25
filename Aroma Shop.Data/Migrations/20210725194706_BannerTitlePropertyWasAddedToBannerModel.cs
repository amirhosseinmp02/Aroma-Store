using Microsoft.EntityFrameworkCore.Migrations;

namespace Aroma_Shop.Data.Migrations
{
    public partial class BannerTitlePropertyWasAddedToBannerModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BannerTitle",
                table: "Banners",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BannerTitle",
                table: "Banners");
        }
    }
}
