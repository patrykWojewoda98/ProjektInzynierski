using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjektIznynierski.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFinancialReport_RemoveReportDate_AddUniqueInstrumentPeriod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FinancialReports_InvestInstrumentId",
                schema: "ProjektInzynierski",
                table: "FinancialReports");

            migrationBuilder.DropColumn(
                name: "ReportDate",
                schema: "ProjektInzynierski",
                table: "FinancialReports");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialReports_InvestInstrumentId_Period",
                schema: "ProjektInzynierski",
                table: "FinancialReports",
                columns: new[] { "InvestInstrumentId", "Period" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FinancialReports_InvestInstrumentId_Period",
                schema: "ProjektInzynierski",
                table: "FinancialReports");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReportDate",
                schema: "ProjektInzynierski",
                table: "FinancialReports",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_FinancialReports_InvestInstrumentId",
                schema: "ProjektInzynierski",
                table: "FinancialReports",
                column: "InvestInstrumentId");
        }
    }
}
