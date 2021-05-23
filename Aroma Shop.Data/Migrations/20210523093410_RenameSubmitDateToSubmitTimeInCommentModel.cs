using Microsoft.EntityFrameworkCore.Migrations;

namespace Aroma_Shop.Data.Migrations
{
    public partial class RenameSubmitDateToSubmitTimeInCommentModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubmitDate",
                table: "Comments",
                newName: "SubmitTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubmitTime",
                table: "Comments",
                newName: "SubmitDate");
        }
    }
}
