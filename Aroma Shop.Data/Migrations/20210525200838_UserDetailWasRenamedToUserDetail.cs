using Microsoft.EntityFrameworkCore.Migrations;

namespace Aroma_Shop.Data.Migrations
{
    public partial class UserDetailWasRenamedToUserDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UsersDetails_UserDetailId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "UserDetailId",
                table: "AspNetUsers",
                newName: "UserDetailsUserDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_UserDetailId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_UserDetailsUserDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UsersDetails_UserDetailsUserDetailId",
                table: "AspNetUsers",
                column: "UserDetailsUserDetailId",
                principalTable: "UsersDetails",
                principalColumn: "UserDetailId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UsersDetails_UserDetailsUserDetailId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "UserDetailsUserDetailId",
                table: "AspNetUsers",
                newName: "UserDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_UserDetailsUserDetailId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_UserDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UsersDetails_UserDetailId",
                table: "AspNetUsers",
                column: "UserDetailId",
                principalTable: "UsersDetails",
                principalColumn: "UserDetailId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
