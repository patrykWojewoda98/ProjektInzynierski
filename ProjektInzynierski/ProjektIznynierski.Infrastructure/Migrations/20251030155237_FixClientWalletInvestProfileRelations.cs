using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjektIznynierski.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixClientWalletInvestProfileRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Wallets_WalletID",
                schema: "ProjektInzynierski",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Clients_WalletID",
                schema: "ProjektInzynierski",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "InvestProfileId",
                schema: "ProjektInzynierski",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "WalletID",
                schema: "ProjektInzynierski",
                table: "Clients");

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                schema: "ProjektInzynierski",
                table: "Wallets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_ClientId",
                schema: "ProjektInzynierski",
                table: "Wallets",
                column: "ClientId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Wallets_Clients_ClientId",
                schema: "ProjektInzynierski",
                table: "Wallets",
                column: "ClientId",
                principalSchema: "ProjektInzynierski",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wallets_Clients_ClientId",
                schema: "ProjektInzynierski",
                table: "Wallets");

            migrationBuilder.DropIndex(
                name: "IX_Wallets_ClientId",
                schema: "ProjektInzynierski",
                table: "Wallets");

            migrationBuilder.DropColumn(
                name: "ClientId",
                schema: "ProjektInzynierski",
                table: "Wallets");

            migrationBuilder.AddColumn<int>(
                name: "InvestProfileId",
                schema: "ProjektInzynierski",
                table: "Clients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WalletID",
                schema: "ProjektInzynierski",
                table: "Clients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_WalletID",
                schema: "ProjektInzynierski",
                table: "Clients",
                column: "WalletID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Wallets_WalletID",
                schema: "ProjektInzynierski",
                table: "Clients",
                column: "WalletID",
                principalSchema: "ProjektInzynierski",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
