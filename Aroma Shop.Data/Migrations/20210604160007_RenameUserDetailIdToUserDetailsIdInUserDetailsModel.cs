using Microsoft.EntityFrameworkCore.Migrations;

namespace Aroma_Shop.Data.Migrations
{
    public partial class RenameUserDetailIdToUserDetailsIdInUserDetailsModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UsersDetails_UserDetailsUserDetailId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "UserDetailId",
                table: "UsersDetails",
                newName: "UserDetailsId");

            migrationBuilder.RenameColumn(
                name: "UserDetailsUserDetailId",
                table: "AspNetUsers",
                newName: "UserDetailsId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_UserDetailsUserDetailId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_UserDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UsersDetails_UserDetailsId",
                table: "AspNetUsers",
                column: "UserDetailsId",
                principalTable: "UsersDetails",
                principalColumn: "UserDetailsId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UsersDetails_UserDetailsId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "UserDetailsId",
                table: "UsersDetails",
                newName: "UserDetailId");

            migrationBuilder.RenameColumn(
                name: "UserDetailsId",
                table: "AspNetUsers",
                newName: "UserDetailsUserDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_UserDetailsId",
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
    }
}
