using Microsoft.EntityFrameworkCore.Migrations;

namespace Aroma_Shop.Data.Migrations
{
    public partial class RenamePageLinkToPagePathAddressInPageModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PageLink",
                table: "Pages",
                newName: "PagePathAddress");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PagePathAddress",
                table: "Pages",
                newName: "PageLink");
        }
    }
}
