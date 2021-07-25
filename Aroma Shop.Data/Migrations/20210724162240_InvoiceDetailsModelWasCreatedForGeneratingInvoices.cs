using Microsoft.EntityFrameworkCore.Migrations;

namespace Aroma_Shop.Data.Migrations
{
    public partial class InvoiceDetailsModelWasCreatedForGeneratingInvoices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InvoiceDetails",
                columns: table => new
                {
                    InvoiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsInvoiceDetailsProductSimple = table.Column<bool>(type: "bit", nullable: false),
                    InvoiceDetailsProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InvoiceDetailsTotalPrice = table.Column<int>(type: "int", nullable: false),
                    InvoiceDetailsQuantity = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    InvoiceDetailsProductAttributesNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvoiceDetailsProductVariationValues = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceDetails", x => x.InvoiceId);
                    table.ForeignKey(
                        name: "FK_InvoiceDetails_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceDetails_OrderId",
                table: "InvoiceDetails",
                column: "OrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceDetails");
        }
    }
}
