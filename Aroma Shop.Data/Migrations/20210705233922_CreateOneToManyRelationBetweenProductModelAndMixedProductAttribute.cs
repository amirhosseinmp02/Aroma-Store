using Microsoft.EntityFrameworkCore.Migrations;

namespace Aroma_Shop.Data.Migrations
{
    public partial class CreateOneToManyRelationBetweenProductModelAndMixedProductAttribute : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "MixedProductAttributes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MixedProductAttributes_ProductId",
                table: "MixedProductAttributes",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_MixedProductAttributes_Products_ProductId",
                table: "MixedProductAttributes",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MixedProductAttributes_Products_ProductId",
                table: "MixedProductAttributes");

            migrationBuilder.DropIndex(
                name: "IX_MixedProductAttributes_ProductId",
                table: "MixedProductAttributes");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "MixedProductAttributes");
        }
    }
}
