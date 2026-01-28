using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjektIznynierski.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCommentsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Comments",
                schema: "ProjektInzynierski",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientID = table.Column<int>(type: "int", nullable: false),
                    InvestInstrumentID = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Clients_ClientID",
                        column: x => x.ClientID,
                        principalSchema: "ProjektInzynierski",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_InvestInstruments_InvestInstrumentID",
                        column: x => x.InvestInstrumentID,
                        principalSchema: "ProjektInzynierski",
                        principalTable: "InvestInstruments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ClientID",
                schema: "ProjektInzynierski",
                table: "Comments",
                column: "ClientID");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_InvestInstrumentID",
                schema: "ProjektInzynierski",
                table: "Comments",
                column: "InvestInstrumentID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments",
                schema: "ProjektInzynierski");
        }
    }
}
