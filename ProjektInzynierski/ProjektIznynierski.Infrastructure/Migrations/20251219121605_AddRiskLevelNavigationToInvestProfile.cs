using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjektIznynierski.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRiskLevelNavigationToInvestProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_InvestProfiles_AcceptableRiskLevelId",
                schema: "ProjektInzynierski",
                table: "InvestProfiles",
                column: "AcceptableRiskLevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvestProfiles_RiskLevel_AcceptableRiskLevelId",
                schema: "ProjektInzynierski",
                table: "InvestProfiles",
                column: "AcceptableRiskLevelId",
                principalSchema: "ProjektInzynierski",
                principalTable: "RiskLevel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvestProfiles_RiskLevel_AcceptableRiskLevelId",
                schema: "ProjektInzynierski",
                table: "InvestProfiles");

            migrationBuilder.DropIndex(
                name: "IX_InvestProfiles_AcceptableRiskLevelId",
                schema: "ProjektInzynierski",
                table: "InvestProfiles");
        }
    }
}
