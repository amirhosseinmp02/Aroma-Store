using Microsoft.EntityFrameworkCore.Migrations;

namespace Aroma_Shop.Data.Migrations
{
    public partial class add_userdetail_property_and_connect_userdetail_to_customidentityuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserDetailId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UsersDetails",
                columns: table => new
                {
                    UserDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    UserProvince = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UserCity = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    UserAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserZipCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersDetails", x => x.UserDetailId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserDetailId",
                table: "AspNetUsers",
                column: "UserDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UsersDetails_UserDetailId",
                table: "AspNetUsers",
                column: "UserDetailId",
                principalTable: "UsersDetails",
                principalColumn: "UserDetailId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UsersDetails_UserDetailId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "UsersDetails");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserDetailId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserDetailId",
                table: "AspNetUsers");
        }
    }
}
