using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjektIznynierski.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ConvertEnumsToEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcceptableRisk",
                schema: "ProjektInzynierski",
                table: "InvestProfiles");

            migrationBuilder.RenameColumn(
                name: "Type",
                schema: "ProjektInzynierski",
                table: "TradeHistories",
                newName: "TradeTypeId");

            migrationBuilder.RenameColumn(
                name: "RegionRisk",
                schema: "ProjektInzynierski",
                table: "Regions",
                newName: "RegionRiskLevelId");

            migrationBuilder.RenameColumn(
                name: "Code",
                schema: "ProjektInzynierski",
                table: "Regions",
                newName: "RegionCodeId");

            migrationBuilder.RenameColumn(
                name: "InvestHorizon",
                schema: "ProjektInzynierski",
                table: "InvestProfiles",
                newName: "AcceptableRiskLevelId");

            migrationBuilder.RenameColumn(
                name: "Type",
                schema: "ProjektInzynierski",
                table: "InvestInstruments",
                newName: "InvestmentTypeId");

            migrationBuilder.RenameColumn(
                name: "CurrencyRisk",
                schema: "ProjektInzynierski",
                table: "Currencies",
                newName: "CurrencyRiskLevelId");

            migrationBuilder.RenameColumn(
                name: "CountryRisk",
                schema: "ProjektInzynierski",
                table: "Countries",
                newName: "CountryRiskLevelId");

            migrationBuilder.AddColumn<int>(
                name: "InvestHorizonId",
                schema: "ProjektInzynierski",
                table: "InvestProfiles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "InvestHorizon",
                schema: "ProjektInzynierski",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Horizon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestHorizon", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InvestmentType",
                schema: "ProjektInzynierski",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegionCode",
                schema: "ProjektInzynierski",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionCode", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RiskLevel",
                schema: "ProjektInzynierski",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RiskLevelScale = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskLevel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TradeType",
                schema: "ProjektInzynierski",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TradeTypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvestProfiles_InvestHorizonId",
                schema: "ProjektInzynierski",
                table: "InvestProfiles",
                column: "InvestHorizonId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvestProfiles_InvestHorizon_InvestHorizonId",
                schema: "ProjektInzynierski",
                table: "InvestProfiles",
                column: "InvestHorizonId",
                principalSchema: "ProjektInzynierski",
                principalTable: "InvestHorizon",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvestProfiles_InvestHorizon_InvestHorizonId",
                schema: "ProjektInzynierski",
                table: "InvestProfiles");

            migrationBuilder.DropTable(
                name: "InvestHorizon",
                schema: "ProjektInzynierski");

            migrationBuilder.DropTable(
                name: "InvestmentType",
                schema: "ProjektInzynierski");

            migrationBuilder.DropTable(
                name: "RegionCode",
                schema: "ProjektInzynierski");

            migrationBuilder.DropTable(
                name: "RiskLevel",
                schema: "ProjektInzynierski");

            migrationBuilder.DropTable(
                name: "TradeType",
                schema: "ProjektInzynierski");

            migrationBuilder.DropIndex(
                name: "IX_InvestProfiles_InvestHorizonId",
                schema: "ProjektInzynierski",
                table: "InvestProfiles");

            migrationBuilder.DropColumn(
                name: "InvestHorizonId",
                schema: "ProjektInzynierski",
                table: "InvestProfiles");

            migrationBuilder.RenameColumn(
                name: "TradeTypeId",
                schema: "ProjektInzynierski",
                table: "TradeHistories",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "RegionRiskLevelId",
                schema: "ProjektInzynierski",
                table: "Regions",
                newName: "RegionRisk");

            migrationBuilder.RenameColumn(
                name: "RegionCodeId",
                schema: "ProjektInzynierski",
                table: "Regions",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "AcceptableRiskLevelId",
                schema: "ProjektInzynierski",
                table: "InvestProfiles",
                newName: "InvestHorizon");

            migrationBuilder.RenameColumn(
                name: "InvestmentTypeId",
                schema: "ProjektInzynierski",
                table: "InvestInstruments",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "CurrencyRiskLevelId",
                schema: "ProjektInzynierski",
                table: "Currencies",
                newName: "CurrencyRisk");

            migrationBuilder.RenameColumn(
                name: "CountryRiskLevelId",
                schema: "ProjektInzynierski",
                table: "Countries",
                newName: "CountryRisk");

            migrationBuilder.AddColumn<int>(
                name: "AcceptableRisk",
                schema: "ProjektInzynierski",
                table: "InvestProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
