using Microsoft.EntityFrameworkCore.Migrations;

namespace Aroma_Shop.Data.Migrations
{
    public partial class AddParentCommentToCommentModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Comments_CommentId1",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "CommentId1",
                table: "Comments",
                newName: "ParentCommentCommentId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_CommentId1",
                table: "Comments",
                newName: "IX_Comments_ParentCommentCommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Comments_ParentCommentCommentId",
                table: "Comments",
                column: "ParentCommentCommentId",
                principalTable: "Comments",
                principalColumn: "CommentId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Comments_ParentCommentCommentId",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "ParentCommentCommentId",
                table: "Comments",
                newName: "CommentId1");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_ParentCommentCommentId",
                table: "Comments",
                newName: "IX_Comments_CommentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Comments_CommentId1",
                table: "Comments",
                column: "CommentId1",
                principalTable: "Comments",
                principalColumn: "CommentId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
