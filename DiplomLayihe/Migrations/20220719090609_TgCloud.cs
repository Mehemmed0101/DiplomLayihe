using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiplomLayihe.Migrations
{
    public partial class TgCloud : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BlogPostsId",
                table: "BlogPosts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PostTagId",
                table: "BlogPosts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PostTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedById = table.Column<int>(type: "int", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostTags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BlogPostTagCloud",
                columns: table => new
                {
                    BlogPostId = table.Column<int>(type: "int", nullable: false),
                    PostTagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPostTagCloud", x => new { x.BlogPostId, x.PostTagId });
                    table.ForeignKey(
                        name: "FK_BlogPostTagCloud_BlogPosts_BlogPostId",
                        column: x => x.BlogPostId,
                        principalTable: "BlogPosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlogPostTagCloud_PostTags_PostTagId",
                        column: x => x.PostTagId,
                        principalTable: "PostTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_BlogPostsId",
                table: "BlogPosts",
                column: "BlogPostsId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_PostTagId",
                table: "BlogPosts",
                column: "PostTagId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPostTagCloud_PostTagId",
                table: "BlogPostTagCloud",
                column: "PostTagId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPosts_BlogPosts_BlogPostsId",
                table: "BlogPosts",
                column: "BlogPostsId",
                principalTable: "BlogPosts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPosts_PostTags_PostTagId",
                table: "BlogPosts",
                column: "PostTagId",
                principalTable: "PostTags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPosts_BlogPosts_BlogPostsId",
                table: "BlogPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogPosts_PostTags_PostTagId",
                table: "BlogPosts");

            migrationBuilder.DropTable(
                name: "BlogPostTagCloud");

            migrationBuilder.DropTable(
                name: "PostTags");

            migrationBuilder.DropIndex(
                name: "IX_BlogPosts_BlogPostsId",
                table: "BlogPosts");

            migrationBuilder.DropIndex(
                name: "IX_BlogPosts_PostTagId",
                table: "BlogPosts");

            migrationBuilder.DropColumn(
                name: "BlogPostsId",
                table: "BlogPosts");

            migrationBuilder.DropColumn(
                name: "PostTagId",
                table: "BlogPosts");
        }
    }
}
