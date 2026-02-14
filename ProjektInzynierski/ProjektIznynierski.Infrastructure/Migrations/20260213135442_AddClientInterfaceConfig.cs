using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjektIznynierski.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddClientInterfaceConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientInterfaceConfigs",
                schema: "ProjektInzynierski",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Platform = table.Column<int>(type: "int", nullable: false),
                    InterfaceType = table.Column<int>(type: "int", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DisplayText = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    OrderIndex = table.Column<int>(type: "int", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedByEmployeeId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientInterfaceConfigs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientInterfaceConfigs_Employees_ModifiedByEmployeeId",
                        column: x => x.ModifiedByEmployeeId,
                        principalSchema: "ProjektInzynierski",
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientInterfaceConfigs_ModifiedByEmployeeId",
                schema: "ProjektInzynierski",
                table: "ClientInterfaceConfigs",
                column: "ModifiedByEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientInterfaceConfigs_Platform_InterfaceType_Key",
                schema: "ProjektInzynierski",
                table: "ClientInterfaceConfigs",
                columns: new[] { "Platform", "InterfaceType", "Key" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientInterfaceConfigs",
                schema: "ProjektInzynierski");
        }
    }
}
