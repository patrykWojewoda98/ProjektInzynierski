using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjektIznynierski.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTradeHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TradeHistories",
                schema: "ProjektInzynierski");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TradeHistories",
                schema: "ProjektInzynierski",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvestInstrumentId = table.Column<int>(type: "int", nullable: false),
                    WalletId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    TradeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TradeTypeId = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TradeHistories_InvestInstruments_InvestInstrumentId",
                        column: x => x.InvestInstrumentId,
                        principalSchema: "ProjektInzynierski",
                        principalTable: "InvestInstruments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TradeHistories_Wallets_WalletId",
                        column: x => x.WalletId,
                        principalSchema: "ProjektInzynierski",
                        principalTable: "Wallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TradeHistories_InvestInstrumentId",
                schema: "ProjektInzynierski",
                table: "TradeHistories",
                column: "InvestInstrumentId");

            migrationBuilder.CreateIndex(
                name: "IX_TradeHistories_WalletId",
                schema: "ProjektInzynierski",
                table: "TradeHistories",
                column: "WalletId");
        }
    }
}
