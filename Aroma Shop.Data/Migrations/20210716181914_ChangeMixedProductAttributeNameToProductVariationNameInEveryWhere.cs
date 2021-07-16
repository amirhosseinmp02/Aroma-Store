using Microsoft.EntityFrameworkCore.Migrations;

namespace Aroma_Shop.Data.Migrations
{
    public partial class ChangeMixedProductAttributeNameToProductVariationNameInEveryWhere : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MixedProductAttributes");

            migrationBuilder.CreateTable(
                name: "ProductVariations",
                columns: table => new
                {
                    ProductVariationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductVariationValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductVariationPrice = table.Column<int>(type: "int", nullable: false),
                    ProductVariationQuantityInStock = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariations", x => x.ProductVariationId);
                    table.ForeignKey(
                        name: "FK_ProductVariations_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariations_ProductId",
                table: "ProductVariations",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductVariations");

            migrationBuilder.CreateTable(
                name: "MixedProductAttributes",
                columns: table => new
                {
                    MixedProductAttributeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MixedProductAttributePrice = table.Column<int>(type: "int", nullable: false),
                    MixedProductAttributeQuantityInStock = table.Column<int>(type: "int", nullable: false),
                    MixedProductAttributeValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixedProductAttributes", x => x.MixedProductAttributeId);
                    table.ForeignKey(
                        name: "FK_MixedProductAttributes_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MixedProductAttributes_ProductId",
                table: "MixedProductAttributes",
                column: "ProductId");
        }
    }
}
