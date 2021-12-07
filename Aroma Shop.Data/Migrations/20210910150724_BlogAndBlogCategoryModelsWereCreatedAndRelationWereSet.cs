using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Aroma_Shop.Data.Migrations
{
    public partial class BlogAndBlogCategoryModelsWereCreatedAndRelationWereSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BlogId",
                table: "Comments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    BlogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlogTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BlogDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BlogShortDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BuilderUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    BlogImageImageId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.BlogId);
                    table.ForeignKey(
                        name: "FK_Blogs_AspNetUsers_BuilderUserId",
                        column: x => x.BuilderUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Blogs_Images_BlogImageImageId",
                        column: x => x.BlogImageImageId,
                        principalTable: "Images",
                        principalColumn: "ImageId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BlogsCategories",
                columns: table => new
                {
                    BlogCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlogCategoryName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    BlogCategoryDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentBlogCategoryBlogCategoryId = table.Column<int>(type: "int", nullable: true),
                    BlogsBlogId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogsCategories", x => x.BlogCategoryId);
                    table.ForeignKey(
                        name: "FK_BlogsCategories_Blogs_BlogsBlogId",
                        column: x => x.BlogsBlogId,
                        principalTable: "Blogs",
                        principalColumn: "BlogId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BlogsCategories_BlogsCategories_ParentBlogCategoryBlogCategoryId",
                        column: x => x.ParentBlogCategoryBlogCategoryId,
                        principalTable: "BlogsCategories",
                        principalColumn: "BlogCategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_BlogId",
                table: "Comments",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_BlogImageImageId",
                table: "Blogs",
                column: "BlogImageImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_BuilderUserId",
                table: "Blogs",
                column: "BuilderUserId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogsCategories_BlogsBlogId",
                table: "BlogsCategories",
                column: "BlogsBlogId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogsCategories_ParentBlogCategoryBlogCategoryId",
                table: "BlogsCategories",
                column: "ParentBlogCategoryBlogCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Blogs_BlogId",
                table: "Comments",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "BlogId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Blogs_BlogId",
                table: "Comments");

            migrationBuilder.DropTable(
                name: "BlogsCategories");

            migrationBuilder.DropTable(
                name: "Blogs");

            migrationBuilder.DropIndex(
                name: "IX_Comments_BlogId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "BlogId",
                table: "Comments");
        }
    }
}
