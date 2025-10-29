using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjektIznynierski.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ProjektInzynierski");

            migrationBuilder.CreateTable(
                name: "Currencies",
                schema: "ProjektInzynierski",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    CurrencyRisk = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FinancialMetrics",
                schema: "ProjektInzynierski",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PE = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    PB = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    ROE = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    DebtToEquity = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    DividendYield = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialMetrics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                schema: "ProjektInzynierski",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Code = table.Column<int>(type: "int", nullable: false),
                    RegionRisk = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sectors",
                schema: "ProjektInzynierski",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sectors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Wallets",
                schema: "ProjektInzynierski",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CashBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wallets_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalSchema: "ProjektInzynierski",
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                schema: "ProjektInzynierski",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsoCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    RegionId = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    CountryRisk = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Countries_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalSchema: "ProjektInzynierski",
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Countries_Regions_RegionId",
                        column: x => x.RegionId,
                        principalSchema: "ProjektInzynierski",
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                schema: "ProjektInzynierski",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    WalletID = table.Column<int>(type: "int", nullable: false),
                    InvestProfileId = table.Column<int>(type: "int", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clients_Countries_CountryId",
                        column: x => x.CountryId,
                        principalSchema: "ProjektInzynierski",
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Clients_Wallets_WalletID",
                        column: x => x.WalletID,
                        principalSchema: "ProjektInzynierski",
                        principalTable: "Wallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvestInstruments",
                schema: "ProjektInzynierski",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Ticker = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    MarketDataDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SectorId = table.Column<int>(type: "int", nullable: false),
                    RegionId = table.Column<int>(type: "int", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    FinancialMetricId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestInstruments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestInstruments_Countries_CountryId",
                        column: x => x.CountryId,
                        principalSchema: "ProjektInzynierski",
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvestInstruments_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalSchema: "ProjektInzynierski",
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvestInstruments_FinancialMetrics_FinancialMetricId",
                        column: x => x.FinancialMetricId,
                        principalSchema: "ProjektInzynierski",
                        principalTable: "FinancialMetrics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_InvestInstruments_Regions_RegionId",
                        column: x => x.RegionId,
                        principalSchema: "ProjektInzynierski",
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvestInstruments_Sectors_SectorId",
                        column: x => x.SectorId,
                        principalSchema: "ProjektInzynierski",
                        principalTable: "Sectors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AIAnalysisResults",
                schema: "ProjektInzynierski",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Summary = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Recommendation = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    KeyInsights = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    ConfidenceScore = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AIAnalysisResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AIAnalysisResults_Clients_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "ProjektInzynierski",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvestProfiles",
                schema: "ProjektInzynierski",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfileName = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    AcceptableRisk = table.Column<int>(type: "int", nullable: false),
                    InvestHorizon = table.Column<int>(type: "int", nullable: false),
                    TargetReturn = table.Column<double>(type: "float", nullable: true),
                    MaxDrawDown = table.Column<double>(type: "float", nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestProfiles_Clients_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "ProjektInzynierski",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WatchLists",
                schema: "ProjektInzynierski",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WatchLists_Clients_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "ProjektInzynierski",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FinancialReports",
                schema: "ProjektInzynierski",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvestInstrumentId = table.Column<int>(type: "int", nullable: false),
                    ReportDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Period = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Revenue = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    NetIncome = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    EPS = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    Assets = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    Liabilities = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    OperatingCashFlow = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    FreeCashFlow = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinancialReports_InvestInstruments_InvestInstrumentId",
                        column: x => x.InvestInstrumentId,
                        principalSchema: "ProjektInzynierski",
                        principalTable: "InvestInstruments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MarketData",
                schema: "ProjektInzynierski",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvestInstrumentId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    OpenPrice = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ClosePrice = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    HighPrice = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    LowPrice = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Volume = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarketData_InvestInstruments_InvestInstrumentId",
                        column: x => x.InvestInstrumentId,
                        principalSchema: "ProjektInzynierski",
                        principalTable: "InvestInstruments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TradeHistories",
                schema: "ProjektInzynierski",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WalletId = table.Column<int>(type: "int", nullable: false),
                    InvestInstrumentId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    TradeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "WalletInstruments",
                schema: "ProjektInzynierski",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WalletId = table.Column<int>(type: "int", nullable: false),
                    InvestInstrumentId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletInstruments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WalletInstruments_InvestInstruments_InvestInstrumentId",
                        column: x => x.InvestInstrumentId,
                        principalSchema: "ProjektInzynierski",
                        principalTable: "InvestInstruments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WalletInstruments_Wallets_WalletId",
                        column: x => x.WalletId,
                        principalSchema: "ProjektInzynierski",
                        principalTable: "Wallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvestProfileRegions",
                schema: "ProjektInzynierski",
                columns: table => new
                {
                    InvestProfileId = table.Column<int>(type: "int", nullable: false),
                    PreferredRegionsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestProfileRegions", x => new { x.InvestProfileId, x.PreferredRegionsId });
                    table.ForeignKey(
                        name: "FK_InvestProfileRegions_InvestProfiles_InvestProfileId",
                        column: x => x.InvestProfileId,
                        principalSchema: "ProjektInzynierski",
                        principalTable: "InvestProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvestProfileRegions_Regions_PreferredRegionsId",
                        column: x => x.PreferredRegionsId,
                        principalSchema: "ProjektInzynierski",
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvestProfileSectors",
                schema: "ProjektInzynierski",
                columns: table => new
                {
                    InvestProfileId = table.Column<int>(type: "int", nullable: false),
                    PreferredSectorsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestProfileSectors", x => new { x.InvestProfileId, x.PreferredSectorsId });
                    table.ForeignKey(
                        name: "FK_InvestProfileSectors_InvestProfiles_InvestProfileId",
                        column: x => x.InvestProfileId,
                        principalSchema: "ProjektInzynierski",
                        principalTable: "InvestProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvestProfileSectors_Sectors_PreferredSectorsId",
                        column: x => x.PreferredSectorsId,
                        principalSchema: "ProjektInzynierski",
                        principalTable: "Sectors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WatchListItems",
                schema: "ProjektInzynierski",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WatchListId = table.Column<int>(type: "int", nullable: false),
                    InvestInstrumentId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchListItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WatchListItems_InvestInstruments_InvestInstrumentId",
                        column: x => x.InvestInstrumentId,
                        principalSchema: "ProjektInzynierski",
                        principalTable: "InvestInstruments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WatchListItems_WatchLists_WatchListId",
                        column: x => x.WatchListId,
                        principalSchema: "ProjektInzynierski",
                        principalTable: "WatchLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AIAnalysisRequests",
                schema: "ProjektInzynierski",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FinancialReportId = table.Column<int>(type: "int", nullable: false),
                    InvestProfileId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AIAnalysisRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AIAnalysisRequests_FinancialReports_FinancialReportId",
                        column: x => x.FinancialReportId,
                        principalSchema: "ProjektInzynierski",
                        principalTable: "FinancialReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AIAnalysisRequests_InvestProfiles_InvestProfileId",
                        column: x => x.InvestProfileId,
                        principalSchema: "ProjektInzynierski",
                        principalTable: "InvestProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AIAnalysisRequests_FinancialReportId",
                schema: "ProjektInzynierski",
                table: "AIAnalysisRequests",
                column: "FinancialReportId");

            migrationBuilder.CreateIndex(
                name: "IX_AIAnalysisRequests_InvestProfileId",
                schema: "ProjektInzynierski",
                table: "AIAnalysisRequests",
                column: "InvestProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_AIAnalysisResults_ClientId",
                schema: "ProjektInzynierski",
                table: "AIAnalysisResults",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_CountryId",
                schema: "ProjektInzynierski",
                table: "Clients",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_Email",
                schema: "ProjektInzynierski",
                table: "Clients",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_WalletID",
                schema: "ProjektInzynierski",
                table: "Clients",
                column: "WalletID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_CurrencyId",
                schema: "ProjektInzynierski",
                table: "Countries",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_IsoCode",
                schema: "ProjektInzynierski",
                table: "Countries",
                column: "IsoCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_RegionId",
                schema: "ProjektInzynierski",
                table: "Countries",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_Name",
                schema: "ProjektInzynierski",
                table: "Currencies",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FinancialReports_InvestInstrumentId",
                schema: "ProjektInzynierski",
                table: "FinancialReports",
                column: "InvestInstrumentId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestInstruments_CountryId",
                schema: "ProjektInzynierski",
                table: "InvestInstruments",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestInstruments_CurrencyId",
                schema: "ProjektInzynierski",
                table: "InvestInstruments",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestInstruments_FinancialMetricId",
                schema: "ProjektInzynierski",
                table: "InvestInstruments",
                column: "FinancialMetricId",
                unique: true,
                filter: "[FinancialMetricId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_InvestInstruments_Name",
                schema: "ProjektInzynierski",
                table: "InvestInstruments",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_InvestInstruments_RegionId",
                schema: "ProjektInzynierski",
                table: "InvestInstruments",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestInstruments_SectorId",
                schema: "ProjektInzynierski",
                table: "InvestInstruments",
                column: "SectorId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestProfileRegions_PreferredRegionsId",
                schema: "ProjektInzynierski",
                table: "InvestProfileRegions",
                column: "PreferredRegionsId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestProfiles_ClientId",
                schema: "ProjektInzynierski",
                table: "InvestProfiles",
                column: "ClientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InvestProfileSectors_PreferredSectorsId",
                schema: "ProjektInzynierski",
                table: "InvestProfileSectors",
                column: "PreferredSectorsId");

            migrationBuilder.CreateIndex(
                name: "IX_MarketData_InvestInstrumentId",
                schema: "ProjektInzynierski",
                table: "MarketData",
                column: "InvestInstrumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Regions_Name",
                schema: "ProjektInzynierski",
                table: "Regions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sectors_Code",
                schema: "ProjektInzynierski",
                table: "Sectors",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sectors_Name",
                schema: "ProjektInzynierski",
                table: "Sectors",
                column: "Name",
                unique: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_WalletInstruments_InvestInstrumentId",
                schema: "ProjektInzynierski",
                table: "WalletInstruments",
                column: "InvestInstrumentId");

            migrationBuilder.CreateIndex(
                name: "IX_WalletInstruments_WalletId",
                schema: "ProjektInzynierski",
                table: "WalletInstruments",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_CurrencyId",
                schema: "ProjektInzynierski",
                table: "Wallets",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_WatchListItems_InvestInstrumentId",
                schema: "ProjektInzynierski",
                table: "WatchListItems",
                column: "InvestInstrumentId");

            migrationBuilder.CreateIndex(
                name: "IX_WatchListItems_WatchListId",
                schema: "ProjektInzynierski",
                table: "WatchListItems",
                column: "WatchListId");

            migrationBuilder.CreateIndex(
                name: "IX_WatchLists_ClientId",
                schema: "ProjektInzynierski",
                table: "WatchLists",
                column: "ClientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AIAnalysisRequests",
                schema: "ProjektInzynierski");

            migrationBuilder.DropTable(
                name: "AIAnalysisResults",
                schema: "ProjektInzynierski");

            migrationBuilder.DropTable(
                name: "InvestProfileRegions",
                schema: "ProjektInzynierski");

            migrationBuilder.DropTable(
                name: "InvestProfileSectors",
                schema: "ProjektInzynierski");

            migrationBuilder.DropTable(
                name: "MarketData",
                schema: "ProjektInzynierski");

            migrationBuilder.DropTable(
                name: "TradeHistories",
                schema: "ProjektInzynierski");

            migrationBuilder.DropTable(
                name: "WalletInstruments",
                schema: "ProjektInzynierski");

            migrationBuilder.DropTable(
                name: "WatchListItems",
                schema: "ProjektInzynierski");

            migrationBuilder.DropTable(
                name: "FinancialReports",
                schema: "ProjektInzynierski");

            migrationBuilder.DropTable(
                name: "InvestProfiles",
                schema: "ProjektInzynierski");

            migrationBuilder.DropTable(
                name: "WatchLists",
                schema: "ProjektInzynierski");

            migrationBuilder.DropTable(
                name: "InvestInstruments",
                schema: "ProjektInzynierski");

            migrationBuilder.DropTable(
                name: "Clients",
                schema: "ProjektInzynierski");

            migrationBuilder.DropTable(
                name: "FinancialMetrics",
                schema: "ProjektInzynierski");

            migrationBuilder.DropTable(
                name: "Sectors",
                schema: "ProjektInzynierski");

            migrationBuilder.DropTable(
                name: "Countries",
                schema: "ProjektInzynierski");

            migrationBuilder.DropTable(
                name: "Wallets",
                schema: "ProjektInzynierski");

            migrationBuilder.DropTable(
                name: "Regions",
                schema: "ProjektInzynierski");

            migrationBuilder.DropTable(
                name: "Currencies",
                schema: "ProjektInzynierski");
        }
    }
}
