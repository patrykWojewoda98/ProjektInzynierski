using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjektIznynierski.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RebuildAiAnalysisRequestTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AIAnalysisRequests_FinancialReports_FinancialReportId",
                schema: "ProjektInzynierski",
                table: "AIAnalysisRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_AIAnalysisRequests_InvestProfiles_InvestProfileId",
                schema: "ProjektInzynierski",
                table: "AIAnalysisRequests");

            migrationBuilder.DropIndex(
                name: "IX_AIAnalysisRequests_FinancialReportId",
                schema: "ProjektInzynierski",
                table: "AIAnalysisRequests");

            migrationBuilder.DropIndex(
                name: "IX_AIAnalysisRequests_InvestProfileId",
                schema: "ProjektInzynierski",
                table: "AIAnalysisRequests");

            migrationBuilder.RenameColumn(
                name: "InvestProfileId",
                schema: "ProjektInzynierski",
                table: "AIAnalysisRequests",
                newName: "InvestInstrumentId");

            migrationBuilder.RenameColumn(
                name: "FinancialReportId",
                schema: "ProjektInzynierski",
                table: "AIAnalysisRequests",
                newName: "ClientId");

            migrationBuilder.AddColumn<int>(
                name: "AIAnalysisResultId",
                schema: "ProjektInzynierski",
                table: "AIAnalysisRequests",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AIAnalysisRequests_AIAnalysisResultId",
                schema: "ProjektInzynierski",
                table: "AIAnalysisRequests",
                column: "AIAnalysisResultId");

            migrationBuilder.AddForeignKey(
                name: "FK_AIAnalysisRequests_AIAnalysisResults_AIAnalysisResultId",
                schema: "ProjektInzynierski",
                table: "AIAnalysisRequests",
                column: "AIAnalysisResultId",
                principalSchema: "ProjektInzynierski",
                principalTable: "AIAnalysisResults",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AIAnalysisRequests_AIAnalysisResults_AIAnalysisResultId",
                schema: "ProjektInzynierski",
                table: "AIAnalysisRequests");

            migrationBuilder.DropIndex(
                name: "IX_AIAnalysisRequests_AIAnalysisResultId",
                schema: "ProjektInzynierski",
                table: "AIAnalysisRequests");

            migrationBuilder.DropColumn(
                name: "AIAnalysisResultId",
                schema: "ProjektInzynierski",
                table: "AIAnalysisRequests");

            migrationBuilder.RenameColumn(
                name: "InvestInstrumentId",
                schema: "ProjektInzynierski",
                table: "AIAnalysisRequests",
                newName: "InvestProfileId");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                schema: "ProjektInzynierski",
                table: "AIAnalysisRequests",
                newName: "FinancialReportId");

            migrationBuilder.CreateIndex(
                name: "IX_AIAnalysisRequests_FinancialReportId",
                schema: "ProjektInzynierski",
                table: "AIAnalysisRequests",
                column: "FinancialReportId");

            migrationBuilder.CreateIndex(
                name: "IX_AIAnalysisRequests_InvestProfileId",
                schema: "ProjektInzynierski",
                table: "AIAnalysisRequests",
                column: "InvestProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_AIAnalysisRequests_FinancialReports_FinancialReportId",
                schema: "ProjektInzynierski",
                table: "AIAnalysisRequests",
                column: "FinancialReportId",
                principalSchema: "ProjektInzynierski",
                principalTable: "FinancialReports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AIAnalysisRequests_InvestProfiles_InvestProfileId",
                schema: "ProjektInzynierski",
                table: "AIAnalysisRequests",
                column: "InvestProfileId",
                principalSchema: "ProjektInzynierski",
                principalTable: "InvestProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
