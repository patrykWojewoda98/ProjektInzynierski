using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjektIznynierski.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeWithTwoFactorAuth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TwoFactorCodeExpiresAt",
                schema: "ProjektInzynierski",
                table: "Employees",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TwoFactorCodeHash",
                schema: "ProjektInzynierski",
                table: "Employees",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TwoFactorCodeExpiresAt",
                schema: "ProjektInzynierski",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "TwoFactorCodeHash",
                schema: "ProjektInzynierski",
                table: "Employees");
        }
    }
}
