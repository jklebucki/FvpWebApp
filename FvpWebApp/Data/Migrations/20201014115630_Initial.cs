using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FvpWebApp.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SourceTypes",
                columns: table => new
                {
                    SourceTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descryption = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SourceTypes", x => x.SourceTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Targets",
                columns: table => new
                {
                    TargetId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descryption = table.Column<string>(nullable: true),
                    DatabaseName = table.Column<string>(nullable: true),
                    DatabaseAddress = table.Column<string>(nullable: true),
                    DatabaseUsername = table.Column<string>(nullable: true),
                    DatabasePassword = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Targets", x => x.TargetId);
                });

            migrationBuilder.CreateTable(
                name: "TaskTickets",
                columns: table => new
                {
                    TaskTicketId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourceId = table.Column<int>(nullable: false),
                    DateFrom = table.Column<DateTime>(nullable: false),
                    DateTo = table.Column<DateTime>(nullable: false),
                    TikcketStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskTickets", x => x.TaskTicketId);
                });

            migrationBuilder.CreateTable(
                name: "Sources",
                columns: table => new
                {
                    SourceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TargetId = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    DbName = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sources", x => x.SourceId);
                    table.ForeignKey(
                        name: "FK_Sources_Targets_TargetId",
                        column: x => x.TargetId,
                        principalTable: "Targets",
                        principalColumn: "TargetId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Contractors",
                columns: table => new
                {
                    ContractorId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractorErpId = table.Column<int>(nullable: true),
                    SourceId = table.Column<int>(nullable: true),
                    ContractorSourceId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    VatId = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    PostCode = table.Column<string>(nullable: true),
                    CountryCode = table.Column<string>(nullable: true),
                    StreetAndNumber = table.Column<string>(nullable: true),
                    ContractorValid = table.Column<bool>(nullable: false),
                    CheckDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contractors", x => x.ContractorId);
                    table.ForeignKey(
                        name: "FK_Contractors_Sources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Sources",
                        principalColumn: "SourceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    DocumentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<int>(nullable: false),
                    SourceId = table.Column<int>(nullable: true),
                    ContractorId = table.Column<int>(nullable: true),
                    DocumentNumber = table.Column<string>(nullable: true),
                    DocumentSymbol = table.Column<string>(nullable: true),
                    SaleDate = table.Column<DateTime>(nullable: false),
                    DocumentDate = table.Column<DateTime>(nullable: false),
                    Net = table.Column<decimal>(type: "decimal(12,4)", nullable: false),
                    Gross = table.Column<decimal>(type: "decimal(12,4)", nullable: false),
                    Vat = table.Column<decimal>(type: "decimal(12,4)", nullable: false),
                    DocumentValid = table.Column<bool>(nullable: false),
                    DocContractorId = table.Column<string>(nullable: true),
                    DocContractorName = table.Column<string>(nullable: true),
                    DocContractorVatCode = table.Column<string>(nullable: true),
                    DocContractorCity = table.Column<string>(nullable: true),
                    DocContractorPostCode = table.Column<string>(nullable: true),
                    DocContractorCountryCode = table.Column<string>(nullable: true),
                    DocContractorStreetAndNumber = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.DocumentId);
                    table.ForeignKey(
                        name: "FK_Documents_Contractors_ContractorId",
                        column: x => x.ContractorId,
                        principalTable: "Contractors",
                        principalColumn: "ContractorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Documents_Sources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Sources",
                        principalColumn: "SourceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DocumentVats",
                columns: table => new
                {
                    DocumentVatId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VatCode = table.Column<string>(nullable: true),
                    VatValue = table.Column<decimal>(type: "decimal(4,2)", nullable: false),
                    VatAmount = table.Column<decimal>(type: "decimal(12,4)", nullable: false),
                    NetAmount = table.Column<decimal>(type: "decimal(12,4)", nullable: false),
                    GrossAmount = table.Column<decimal>(type: "decimal(12,4)", nullable: false),
                    VatTags = table.Column<string>(nullable: true),
                    DocumentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentVats", x => x.DocumentVatId);
                    table.ForeignKey(
                        name: "FK_DocumentVats_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "DocumentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Sources",
                columns: new[] { "SourceId", "Address", "Code", "DbName", "Description", "Password", "TargetId", "Type", "Username" },
                values: new object[,]
                {
                    { 1, "192.168.42.70", "DP1", "sben", "Dyskont Paliwowy Słowiańska", "almarwinnet", null, "oracle_sben_dp", "sben" },
                    { 2, "192.168.45.70", "DP2", "sben", "Dyskont Paliwowy Słowiańska", "almarwinnet", null, "oracle_sben_dp", "sben" },
                    { 3, "I:\\DaneBP\\MOSTKI", "BPMOSTKI", "", "Stacja Paliw BP Mostki", "", null, "bp_flat_file", "" }
                });

            migrationBuilder.InsertData(
                table: "Targets",
                columns: new[] { "TargetId", "DatabaseAddress", "DatabaseName", "DatabasePassword", "DatabaseUsername", "Descryption" },
                values: new object[,]
                {
                    { 1, "192.168.21.20", "CITRONEX_FK", "#sa2015!", "sa", "Citronex I - Symfonia ERP" },
                    { 2, "192.168.21.20", "CITRONEX_MOP", "#sa2015!", "sa", "Citronex MOP - Symfonia ERP" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contractors_SourceId",
                table: "Contractors",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_ContractorId",
                table: "Documents",
                column: "ContractorId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_SourceId",
                table: "Documents",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentVats_DocumentId",
                table: "DocumentVats",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sources_TargetId",
                table: "Sources",
                column: "TargetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentVats");

            migrationBuilder.DropTable(
                name: "SourceTypes");

            migrationBuilder.DropTable(
                name: "TaskTickets");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "Contractors");

            migrationBuilder.DropTable(
                name: "Sources");

            migrationBuilder.DropTable(
                name: "Targets");
        }
    }
}
