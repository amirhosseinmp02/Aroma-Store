using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Aroma_Shop.Data.Migrations
{
    public partial class CreateMessageReplyModel_ChangeRelationBetweenMessageAndItSelfWithCreateRelationBetweenMessageModelAndMessageReplyModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Messages_MessageReplyMessageId",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "MessageReplyMessageId",
                table: "Messages",
                newName: "MessageReplyId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_MessageReplyMessageId",
                table: "Messages",
                newName: "IX_Messages_MessageReplyId");

            migrationBuilder.CreateTable(
                name: "MessagesReplies",
                columns: table => new
                {
                    MessageReplyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageReplySubmitTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MessageReplyDescription = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessagesReplies", x => x.MessageReplyId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_MessagesReplies_MessageReplyId",
                table: "Messages",
                column: "MessageReplyId",
                principalTable: "MessagesReplies",
                principalColumn: "MessageReplyId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_MessagesReplies_MessageReplyId",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "MessagesReplies");

            migrationBuilder.RenameColumn(
                name: "MessageReplyId",
                table: "Messages",
                newName: "MessageReplyMessageId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_MessageReplyId",
                table: "Messages",
                newName: "IX_Messages_MessageReplyMessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Messages_MessageReplyMessageId",
                table: "Messages",
                column: "MessageReplyMessageId",
                principalTable: "Messages",
                principalColumn: "MessageId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
