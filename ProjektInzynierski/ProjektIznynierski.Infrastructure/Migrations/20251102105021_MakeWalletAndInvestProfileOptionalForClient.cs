using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjektIznynierski.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeWalletAndInvestProfileOptionalForClient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Wallets_ClientId",
                schema: "ProjektInzynierski",
                table: "Wallets");

            migrationBuilder.DropIndex(
                name: "IX_InvestProfiles_ClientId",
                schema: "ProjektInzynierski",
                table: "InvestProfiles");

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                schema: "ProjektInzynierski",
                table: "Wallets",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                schema: "ProjektInzynierski",
                table: "InvestProfiles",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_ClientId",
                schema: "ProjektInzynierski",
                table: "Wallets",
                column: "ClientId",
                unique: true,
                filter: "[ClientId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_InvestProfiles_ClientId",
                schema: "ProjektInzynierski",
                table: "InvestProfiles",
                column: "ClientId",
                unique: true,
                filter: "[ClientId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Wallets_ClientId",
                schema: "ProjektInzynierski",
                table: "Wallets");

            migrationBuilder.DropIndex(
                name: "IX_InvestProfiles_ClientId",
                schema: "ProjektInzynierski",
                table: "InvestProfiles");

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                schema: "ProjektInzynierski",
                table: "Wallets",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                schema: "ProjektInzynierski",
                table: "InvestProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_ClientId",
                schema: "ProjektInzynierski",
                table: "Wallets",
                column: "ClientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvestProfiles_ClientId",
                schema: "ProjektInzynierski",
                table: "InvestProfiles",
                column: "ClientId",
                unique: true);
        }
    }
}
