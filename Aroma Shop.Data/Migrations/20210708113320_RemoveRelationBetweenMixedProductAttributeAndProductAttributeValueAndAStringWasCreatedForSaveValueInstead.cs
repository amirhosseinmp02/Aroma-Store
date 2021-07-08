using Microsoft.EntityFrameworkCore.Migrations;

namespace Aroma_Shop.Data.Migrations
{
    public partial class RemoveRelationBetweenMixedProductAttributeAndProductAttributeValueAndAStringWasCreatedForSaveValueInstead : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductAttributeValues_MixedProductAttributes_MixedProductAttributeId",
                table: "ProductAttributeValues");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductAttributeValues_ProductAttributes_ProductAttributeId",
                table: "ProductAttributeValues");

            migrationBuilder.DropIndex(
                name: "IX_ProductAttributeValues_MixedProductAttributeId",
                table: "ProductAttributeValues");

            migrationBuilder.DropColumn(
                name: "MixedProductAttributeId",
                table: "ProductAttributeValues");

            migrationBuilder.AlterColumn<int>(
                name: "ProductAttributeId",
                table: "ProductAttributeValues",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MixedProductAttributeValue",
                table: "MixedProductAttributes",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAttributeValues_ProductAttributes_ProductAttributeId",
                table: "ProductAttributeValues",
                column: "ProductAttributeId",
                principalTable: "ProductAttributes",
                principalColumn: "ProductAttributeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductAttributeValues_ProductAttributes_ProductAttributeId",
                table: "ProductAttributeValues");

            migrationBuilder.DropColumn(
                name: "MixedProductAttributeValue",
                table: "MixedProductAttributes");

            migrationBuilder.AlterColumn<int>(
                name: "ProductAttributeId",
                table: "ProductAttributeValues",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "MixedProductAttributeId",
                table: "ProductAttributeValues",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributeValues_MixedProductAttributeId",
                table: "ProductAttributeValues",
                column: "MixedProductAttributeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAttributeValues_MixedProductAttributes_MixedProductAttributeId",
                table: "ProductAttributeValues",
                column: "MixedProductAttributeId",
                principalTable: "MixedProductAttributes",
                principalColumn: "MixedProductAttributeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAttributeValues_ProductAttributes_ProductAttributeId",
                table: "ProductAttributeValues",
                column: "ProductAttributeId",
                principalTable: "ProductAttributes",
                principalColumn: "ProductAttributeId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
