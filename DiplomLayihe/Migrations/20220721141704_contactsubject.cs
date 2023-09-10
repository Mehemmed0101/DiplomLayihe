using Microsoft.EntityFrameworkCore.Migrations;

namespace DiplomLayihe.Migrations
{
    public partial class contactsubject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlanExplanations_PlanAndPricing_PlanAndPricingId",
                table: "PlanExplanations");

            migrationBuilder.DropIndex(
                name: "IX_PlanExplanations_PlanAndPricingId",
                table: "PlanExplanations");

            migrationBuilder.DropColumn(
                name: "PlanAndPricingId",
                table: "PlanExplanations");

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "ContactUs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PlanAndPricingPlanExplanations",
                columns: table => new
                {
                    PlanAndPricingId = table.Column<int>(type: "int", nullable: false),
                    PlanExplanationsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanAndPricingPlanExplanations", x => new { x.PlanAndPricingId, x.PlanExplanationsId });
                    table.ForeignKey(
                        name: "FK_PlanAndPricingPlanExplanations_PlanAndPricing_PlanAndPricingId",
                        column: x => x.PlanAndPricingId,
                        principalTable: "PlanAndPricing",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanAndPricingPlanExplanations_PlanExplanations_PlanExplanationsId",
                        column: x => x.PlanExplanationsId,
                        principalTable: "PlanExplanations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlanAndPricingPlanExplanations_PlanExplanationsId",
                table: "PlanAndPricingPlanExplanations",
                column: "PlanExplanationsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlanAndPricingPlanExplanations");

            migrationBuilder.DropColumn(
                name: "Subject",
                table: "ContactUs");

            migrationBuilder.AddColumn<int>(
                name: "PlanAndPricingId",
                table: "PlanExplanations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlanExplanations_PlanAndPricingId",
                table: "PlanExplanations",
                column: "PlanAndPricingId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlanExplanations_PlanAndPricing_PlanAndPricingId",
                table: "PlanExplanations",
                column: "PlanAndPricingId",
                principalTable: "PlanAndPricing",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
