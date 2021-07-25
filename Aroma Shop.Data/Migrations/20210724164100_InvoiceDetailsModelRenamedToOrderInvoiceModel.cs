using Microsoft.EntityFrameworkCore.Migrations;

namespace Aroma_Shop.Data.Migrations
{
    public partial class InvoiceDetailsModelRenamedToOrderInvoiceModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoicesDetails");

            migrationBuilder.CreateTable(
                name: "OrdersInvoicesDetails",
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
                    table.PrimaryKey("PK_OrdersInvoicesDetails", x => x.InvoiceId);
                    table.ForeignKey(
                        name: "FK_OrdersInvoicesDetails_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrdersInvoicesDetails_OrderId",
                table: "OrdersInvoicesDetails",
                column: "OrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrdersInvoicesDetails");

            migrationBuilder.CreateTable(
                name: "InvoicesDetails",
                columns: table => new
                {
                    InvoiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceDetailsProductAttributesNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvoiceDetailsProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InvoiceDetailsProductVariationValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvoiceDetailsQuantity = table.Column<int>(type: "int", nullable: false),
                    InvoiceDetailsTotalPrice = table.Column<int>(type: "int", nullable: false),
                    IsInvoiceDetailsProductSimple = table.Column<bool>(type: "bit", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoicesDetails", x => x.InvoiceId);
                    table.ForeignKey(
                        name: "FK_InvoicesDetails_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoicesDetails_OrderId",
                table: "InvoicesDetails",
                column: "OrderId");
        }
    }
}
