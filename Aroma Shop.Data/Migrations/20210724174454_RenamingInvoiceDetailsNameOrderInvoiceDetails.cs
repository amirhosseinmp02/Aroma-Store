using Microsoft.EntityFrameworkCore.Migrations;

namespace Aroma_Shop.Data.Migrations
{
    public partial class RenamingInvoiceDetailsNameOrderInvoiceDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdersInvoicesDetails_Orders_OrderId",
                table: "OrdersInvoicesDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrdersInvoicesDetails",
                table: "OrdersInvoicesDetails");

            migrationBuilder.RenameTable(
                name: "OrdersInvoicesDetails",
                newName: "InvoicesDetails");

            migrationBuilder.RenameIndex(
                name: "IX_OrdersInvoicesDetails_OrderId",
                table: "InvoicesDetails",
                newName: "IX_InvoicesDetails_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InvoicesDetails",
                table: "InvoicesDetails",
                column: "InvoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoicesDetails_Orders_OrderId",
                table: "InvoicesDetails",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoicesDetails_Orders_OrderId",
                table: "InvoicesDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InvoicesDetails",
                table: "InvoicesDetails");

            migrationBuilder.RenameTable(
                name: "InvoicesDetails",
                newName: "OrdersInvoicesDetails");

            migrationBuilder.RenameIndex(
                name: "IX_InvoicesDetails_OrderId",
                table: "OrdersInvoicesDetails",
                newName: "IX_OrdersInvoicesDetails_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrdersInvoicesDetails",
                table: "OrdersInvoicesDetails",
                column: "InvoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdersInvoicesDetails_Orders_OrderId",
                table: "OrdersInvoicesDetails",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
