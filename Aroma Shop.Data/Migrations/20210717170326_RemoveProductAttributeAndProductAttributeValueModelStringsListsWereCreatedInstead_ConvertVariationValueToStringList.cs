using Microsoft.EntityFrameworkCore.Migrations;

namespace Aroma_Shop.Data.Migrations
{
    public partial class RemoveProductAttributeAndProductAttributeValueModelStringsListsWereCreatedInstead_ConvertVariationValueToStringList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductAttributeValues");

            migrationBuilder.DropTable(
                name: "ProductAttributes");

            migrationBuilder.RenameColumn(
                name: "ProductVariationValue",
                table: "ProductVariations",
                newName: "ProductVariationValues");

            migrationBuilder.AddColumn<string>(
                name: "ProductAttributesNames",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductAttributesValues",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductAttributesNames",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductAttributesValues",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "ProductVariationValues",
                table: "ProductVariations",
                newName: "ProductVariationValue");

            migrationBuilder.CreateTable(
                name: "ProductAttributes",
                columns: table => new
                {
                    ProductAttributeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductAttributeName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAttributes", x => x.ProductAttributeId);
                    table.ForeignKey(
                        name: "FK_ProductAttributes_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductAttributeValues",
                columns: table => new
                {
                    AttributeValueId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttributeValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductAttributeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAttributeValues", x => x.AttributeValueId);
                    table.ForeignKey(
                        name: "FK_ProductAttributeValues_ProductAttributes_ProductAttributeId",
                        column: x => x.ProductAttributeId,
                        principalTable: "ProductAttributes",
                        principalColumn: "ProductAttributeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributes_ProductId",
                table: "ProductAttributes",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributeValues_ProductAttributeId",
                table: "ProductAttributeValues",
                column: "ProductAttributeId");
        }
    }
}
