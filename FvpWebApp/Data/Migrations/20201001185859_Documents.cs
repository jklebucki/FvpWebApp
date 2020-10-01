using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FvpWebApp.Data.Migrations
{
    public partial class Documents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    DocumentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourceId = table.Column<int>(nullable: false),
                    ExternalId = table.Column<int>(nullable: false),
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
                });

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
                name: "Contractors",
                columns: table => new
                {
                    ContractorId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractorErpId = table.Column<int>(nullable: false),
                    ContractorSourceId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    VatCode = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    PostCode = table.Column<string>(nullable: true),
                    CountryCode = table.Column<string>(nullable: true),
                    StreetAndNumber = table.Column<string>(nullable: true),
                    ContractorValid = table.Column<bool>(nullable: false),
                    CheckDate = table.Column<DateTime>(nullable: true),
                    DocumentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contractors", x => x.ContractorId);
                    table.ForeignKey(
                        name: "FK_Contractors_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "DocumentId",
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

            migrationBuilder.CreateTable(
                name: "Sources",
                columns: table => new
                {
                    SourceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    DbName = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    DocumentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sources", x => x.SourceId);
                    table.ForeignKey(
                        name: "FK_Sources_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "DocumentId",
                        onDelete: ReferentialAction.Restrict);
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
                    DatabasePassword = table.Column<string>(nullable: true),
                    SourceId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Targets", x => x.TargetId);
                    table.ForeignKey(
                        name: "FK_Targets_Sources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Sources",
                        principalColumn: "SourceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contractors_DocumentId",
                table: "Contractors",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentVats_DocumentId",
                table: "DocumentVats",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sources_DocumentId",
                table: "Sources",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Targets_SourceId",
                table: "Targets",
                column: "SourceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contractors");

            migrationBuilder.DropTable(
                name: "DocumentVats");

            migrationBuilder.DropTable(
                name: "SourceTypes");

            migrationBuilder.DropTable(
                name: "Targets");

            migrationBuilder.DropTable(
                name: "Sources");

            migrationBuilder.DropTable(
                name: "Documents");
        }
    }
}
