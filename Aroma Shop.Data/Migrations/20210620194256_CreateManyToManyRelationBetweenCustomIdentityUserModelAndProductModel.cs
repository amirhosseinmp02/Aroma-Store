using Microsoft.EntityFrameworkCore.Migrations;

namespace Aroma_Shop.Data.Migrations
{
    public partial class CreateManyToManyRelationBetweenCustomIdentityUserModelAndProductModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomIdentityUserProduct",
                columns: table => new
                {
                    FavoriteProductsProductId = table.Column<int>(type: "int", nullable: false),
                    InterestedUsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomIdentityUserProduct", x => new { x.FavoriteProductsProductId, x.InterestedUsersId });
                    table.ForeignKey(
                        name: "FK_CustomIdentityUserProduct_AspNetUsers_InterestedUsersId",
                        column: x => x.InterestedUsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomIdentityUserProduct_Products_FavoriteProductsProductId",
                        column: x => x.FavoriteProductsProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomIdentityUserProduct_InterestedUsersId",
                table: "CustomIdentityUserProduct",
                column: "InterestedUsersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomIdentityUserProduct");
        }
    }
}
