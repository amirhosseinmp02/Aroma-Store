using Microsoft.EntityFrameworkCore.Migrations;

namespace Aroma_Shop.Data.Migrations
{
    public partial class SomeOrderModelAndOrderInvoiceDetailsPropertiesWereRenamed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InvoiceId",
                table: "OrdersInvoicesDetails",
                newName: "OrderInvoiceId");

            migrationBuilder.RenameColumn(
                name: "OrderRegistrationDate",
                table: "Orders",
                newName: "OrderPaymentTime");

            migrationBuilder.RenameColumn(
                name: "IsSeen",
                table: "Orders",
                newName: "IsOrderSeen");

            migrationBuilder.RenameColumn(
                name: "IsFinally",
                table: "Orders",
                newName: "IsOrderCompleted");

            migrationBuilder.RenameColumn(
                name: "CreateTime",
                table: "Orders",
                newName: "OrderCreateTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderInvoiceId",
                table: "OrdersInvoicesDetails",
                newName: "InvoiceId");

            migrationBuilder.RenameColumn(
                name: "OrderPaymentTime",
                table: "Orders",
                newName: "OrderRegistrationDate");

            migrationBuilder.RenameColumn(
                name: "OrderCreateTime",
                table: "Orders",
                newName: "CreateTime");

            migrationBuilder.RenameColumn(
                name: "IsOrderSeen",
                table: "Orders",
                newName: "IsSeen");

            migrationBuilder.RenameColumn(
                name: "IsOrderCompleted",
                table: "Orders",
                newName: "IsFinally");
        }
    }
}
