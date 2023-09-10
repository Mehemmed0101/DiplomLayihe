using Microsoft.EntityFrameworkCore.Migrations;

namespace DiplomLayihe.Migrations
{
    public partial class coursetag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPosts_BlogPosts_BlogPostsId",
                table: "BlogPosts");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogPosts_PostTags_PostTagId",
                table: "BlogPosts");

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

            migrationBuilder.CreateTable(
                name: "CourseTagCloud",
                columns: table => new
                {
                    CoursePostId = table.Column<int>(type: "int", nullable: false),
                    PostTagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseTagCloud", x => new { x.CoursePostId, x.PostTagId });
                    table.ForeignKey(
                        name: "FK_CourseTagCloud_CourseCategories_CoursePostId",
                        column: x => x.CoursePostId,
                        principalTable: "CourseCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseTagCloud_PostTags_PostTagId",
                        column: x => x.PostTagId,
                        principalTable: "PostTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseTagCloud_PostTagId",
                table: "CourseTagCloud",
                column: "PostTagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseTagCloud");

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

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_BlogPostsId",
                table: "BlogPosts",
                column: "BlogPostsId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_PostTagId",
                table: "BlogPosts",
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
    }
}
