using Microsoft.EntityFrameworkCore.Migrations;

namespace Aroma_Shop.Data.Migrations
{
    public partial class AddInvoiceDetailsModelToContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceDetails_Orders_OrderId",
                table: "InvoiceDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InvoiceDetails",
                table: "InvoiceDetails");

            migrationBuilder.RenameTable(
                name: "InvoiceDetails",
                newName: "InvoicesDetails");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceDetails_OrderId",
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
                newName: "InvoiceDetails");

            migrationBuilder.RenameIndex(
                name: "IX_InvoicesDetails_OrderId",
                table: "InvoiceDetails",
                newName: "IX_InvoiceDetails_OrderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InvoiceDetails",
                table: "InvoiceDetails",
                column: "InvoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceDetails_Orders_OrderId",
                table: "InvoiceDetails",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
