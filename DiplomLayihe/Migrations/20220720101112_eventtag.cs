using Microsoft.EntityFrameworkCore.Migrations;

namespace DiplomLayihe.Migrations
{
    public partial class eventtag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventTagCloud",
                columns: table => new
                {
                    EventPostId = table.Column<int>(type: "int", nullable: false),
                    PostTagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTagCloud", x => new { x.EventPostId, x.PostTagId });
                    table.ForeignKey(
                        name: "FK_EventTagCloud_LastNewsandEvents_EventPostId",
                        column: x => x.EventPostId,
                        principalTable: "LastNewsandEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventTagCloud_PostTags_PostTagId",
                        column: x => x.PostTagId,
                        principalTable: "PostTags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventTagCloud_PostTagId",
                table: "EventTagCloud",
                column: "PostTagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventTagCloud");
        }
    }
}
