using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjektIznynierski.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrencyPairsAndCurrencyRateHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CurrencyPairs",
                schema: "ProjektInzynierski",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BaseCurrencyId = table.Column<int>(type: "int", nullable: false),
                    QuoteCurrencyId = table.Column<int>(type: "int", nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyPairs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CurrencyPairs_Currencies_BaseCurrencyId",
                        column: x => x.BaseCurrencyId,
                        principalSchema: "ProjektInzynierski",
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CurrencyPairs_Currencies_QuoteCurrencyId",
                        column: x => x.QuoteCurrencyId,
                        principalSchema: "ProjektInzynierski",
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CurrencyRateHistories",
                schema: "ProjektInzynierski",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrencyPairId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CloseRate = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    OpenRate = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: true),
                    HighRate = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: true),
                    LowRate = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyRateHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CurrencyRateHistories_CurrencyPairs_CurrencyPairId",
                        column: x => x.CurrencyPairId,
                        principalSchema: "ProjektInzynierski",
                        principalTable: "CurrencyPairs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyPairs_BaseCurrencyId_QuoteCurrencyId",
                schema: "ProjektInzynierski",
                table: "CurrencyPairs",
                columns: new[] { "BaseCurrencyId", "QuoteCurrencyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyPairs_QuoteCurrencyId",
                schema: "ProjektInzynierski",
                table: "CurrencyPairs",
                column: "QuoteCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyRateHistories_CurrencyPairId_Date",
                schema: "ProjektInzynierski",
                table: "CurrencyRateHistories",
                columns: new[] { "CurrencyPairId", "Date" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrencyRateHistories",
                schema: "ProjektInzynierski");

            migrationBuilder.DropTable(
                name: "CurrencyPairs",
                schema: "ProjektInzynierski");
        }
    }
}
