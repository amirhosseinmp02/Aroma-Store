using Microsoft.EntityFrameworkCore.Migrations;

namespace Aroma_Shop.Data.Migrations
{
    public partial class AddMessageReplyNavigationProperyToMessageModelToCreateRelationWithItSelf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MessageReplyMessageId",
                table: "Messages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_MessageReplyMessageId",
                table: "Messages",
                column: "MessageReplyMessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Messages_MessageReplyMessageId",
                table: "Messages",
                column: "MessageReplyMessageId",
                principalTable: "Messages",
                principalColumn: "MessageId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Messages_MessageReplyMessageId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_MessageReplyMessageId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "MessageReplyMessageId",
                table: "Messages");
        }
    }
}
