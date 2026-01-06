using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjektIznynierski.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixFinancialMetricRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvestInstruments_FinancialMetrics_FinancialMetricId",
                schema: "ProjektInzynierski",
                table: "InvestInstruments");

            migrationBuilder.DropIndex(
                name: "IX_InvestInstruments_FinancialMetricId",
                schema: "ProjektInzynierski",
                table: "InvestInstruments");

            migrationBuilder.AddColumn<int>(
                name: "InvestmentInstrumentId",
                schema: "ProjektInzynierski",
                table: "FinancialMetrics",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FinancialMetrics_InvestmentInstrumentId",
                schema: "ProjektInzynierski",
                table: "FinancialMetrics",
                column: "InvestmentInstrumentId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialMetrics_InvestInstruments_InvestmentInstrumentId",
                schema: "ProjektInzynierski",
                table: "FinancialMetrics",
                column: "InvestmentInstrumentId",
                principalSchema: "ProjektInzynierski",
                principalTable: "InvestInstruments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinancialMetrics_InvestInstruments_InvestmentInstrumentId",
                schema: "ProjektInzynierski",
                table: "FinancialMetrics");

            migrationBuilder.DropIndex(
                name: "IX_FinancialMetrics_InvestmentInstrumentId",
                schema: "ProjektInzynierski",
                table: "FinancialMetrics");

            migrationBuilder.DropColumn(
                name: "InvestmentInstrumentId",
                schema: "ProjektInzynierski",
                table: "FinancialMetrics");

            migrationBuilder.CreateIndex(
                name: "IX_InvestInstruments_FinancialMetricId",
                schema: "ProjektInzynierski",
                table: "InvestInstruments",
                column: "FinancialMetricId",
                unique: true,
                filter: "[FinancialMetricId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_InvestInstruments_FinancialMetrics_FinancialMetricId",
                schema: "ProjektInzynierski",
                table: "InvestInstruments",
                column: "FinancialMetricId",
                principalSchema: "ProjektInzynierski",
                principalTable: "FinancialMetrics",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
