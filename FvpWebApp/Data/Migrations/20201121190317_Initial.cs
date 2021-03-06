﻿using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace FvpWebApp.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Symbol = table.Column<string>(nullable: true),
                    NamePL = table.Column<string>(nullable: true),
                    NameENG = table.Column<string>(nullable: true),
                    Info = table.Column<string>(nullable: true),
                    UE = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryId);
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
                name: "TargetDocumentsSettings",
                columns: table => new
                {
                    TargetDocumentSettingsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourceId = table.Column<int>(nullable: false),
                    DocumentShortcut = table.Column<string>(nullable: true),
                    VatRegisterId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TargetDocumentsSettings", x => x.TargetDocumentSettingsId);
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
                    TicketStatus = table.Column<int>(nullable: false),
                    TicketType = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    StatusChangedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskTickets", x => x.TaskTicketId);
                });

            migrationBuilder.CreateTable(
                name: "VatRegisters",
                columns: table => new
                {
                    VatRegisterId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TargetDocumentSettingsId = table.Column<int>(nullable: false),
                    VatValue = table.Column<decimal>(type: "decimal(12,4)", nullable: false),
                    ErpVatRegisterId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VatRegisters", x => x.VatRegisterId);
                    table.ForeignKey(
                        name: "FK_VatRegisters_TargetDocumentsSettings_TargetDocumentSettingsId",
                        column: x => x.TargetDocumentSettingsId,
                        principalTable: "TargetDocumentsSettings",
                        principalColumn: "TargetDocumentSettingsId",
                        onDelete: ReferentialAction.Cascade);
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
                name: "AccountingRecords",
                columns: table => new
                {
                    AccountingRecordId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourceId = table.Column<int>(nullable: false),
                    RecordOrder = table.Column<int>(nullable: false),
                    Account = table.Column<string>(nullable: true),
                    Debit = table.Column<string>(nullable: true),
                    Credit = table.Column<string>(nullable: true),
                    Sign = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountingRecords", x => x.AccountingRecordId);
                    table.ForeignKey(
                        name: "FK_AccountingRecords_Sources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Sources",
                        principalColumn: "SourceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contractors",
                columns: table => new
                {
                    ContractorId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractorErpId = table.Column<int>(nullable: true),
                    ContractorErpPosition = table.Column<int>(nullable: true),
                    SourceId = table.Column<int>(nullable: true),
                    GusContractorEntriesCount = table.Column<int>(nullable: true),
                    ContractorSourceId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Street = table.Column<string>(nullable: true),
                    EstateNumber = table.Column<string>(nullable: true),
                    QuartersNumber = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    PostalCode = table.Column<string>(nullable: true),
                    Province = table.Column<string>(nullable: true),
                    VatId = table.Column<string>(nullable: true),
                    Regon = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    CountryCode = table.Column<string>(nullable: true),
                    Firm = table.Column<int>(nullable: false),
                    ContractorStatus = table.Column<int>(nullable: false),
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
                    TaskTicketId = table.Column<int>(nullable: false),
                    DocumentNumber = table.Column<string>(nullable: true),
                    DocumentSymbol = table.Column<string>(nullable: true),
                    SaleDate = table.Column<DateTime>(nullable: false),
                    DocumentDate = table.Column<DateTime>(nullable: false),
                    Net = table.Column<decimal>(type: "decimal(12,4)", nullable: false),
                    Gross = table.Column<decimal>(type: "decimal(12,4)", nullable: false),
                    Vat = table.Column<decimal>(type: "decimal(12,4)", nullable: false),
                    JpkV7 = table.Column<string>(nullable: true),
                    DocumentStatus = table.Column<int>(nullable: false),
                    DocContractorId = table.Column<string>(nullable: true),
                    DocContractorName = table.Column<string>(nullable: true),
                    DocContractorVatId = table.Column<string>(nullable: true),
                    DocContractorCity = table.Column<string>(nullable: true),
                    DocContractorPostCode = table.Column<string>(nullable: true),
                    DocContractorCountryCode = table.Column<string>(nullable: true),
                    DocContractorStreetAndNumber = table.Column<string>(nullable: true),
                    DocContractorFirm = table.Column<int>(nullable: false),
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
                    table.ForeignKey(
                        name: "FK_Documents_TaskTickets_TaskTicketId",
                        column: x => x.TaskTicketId,
                        principalTable: "TaskTickets",
                        principalColumn: "TaskTicketId",
                        onDelete: ReferentialAction.Cascade);
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
                table: "Countries",
                columns: new[] { "CountryId", "Info", "NameENG", "NamePL", "Symbol", "UE" },
                values: new object[,]
                {
                    { 1, null, null, "Austria", "AT", true },
                    { 28, null, null, "Słowacja", "SK", true },
                    { 27, null, null, "Słowenia", "SI", true },
                    { 26, null, null, "Szwecja", "SE", true },
                    { 25, null, null, "Rumunia", "RO", true },
                    { 24, null, null, "Portugalia", "PT", true },
                    { 23, null, null, "Polska", "PL", true },
                    { 22, null, null, "Holandia", "NL", true },
                    { 21, null, null, "Malta", "MT", true },
                    { 20, null, null, "Łotwa", "LV", true },
                    { 19, null, null, "Luksemburg", "LU", true },
                    { 18, null, null, "Litwa", "LT", true },
                    { 17, null, null, "Włochy", "IT", true },
                    { 15, null, null, "Węgry", "HU", true },
                    { 16, null, null, "Irlandia", "IE", true },
                    { 6, null, null, "Niemcy", "DE", true },
                    { 13, null, null, "Wielka Brytania", "GB", true },
                    { 12, null, null, "Francja", "FR", true },
                    { 11, null, null, "Finlandia", "FI", true },
                    { 10, null, null, "Hiszpania", "ES", true },
                    { 9, null, null, "Grecja", "EL", true },
                    { 8, null, null, "Estonia", "EE", true },
                    { 7, null, null, "Dania", "DK", true },
                    { 14, null, null, "Chorwacja", "HR", true },
                    { 5, null, null, "Czechy", "CZ", true },
                    { 4, null, null, "Cypr", "CY", true },
                    { 3, null, null, "Bułgaria", "BG", true },
                    { 2, null, null, "Belgia", "BE", true }
                });

            migrationBuilder.InsertData(
                table: "TargetDocumentsSettings",
                columns: new[] { "TargetDocumentSettingsId", "DocumentShortcut", "SourceId", "VatRegisterId" },
                values: new object[,]
                {
                    { 1, "FDZG", 1, 1 },
                    { 2, "FDD2", 2, 1 }
                });

            migrationBuilder.InsertData(
                table: "Targets",
                columns: new[] { "TargetId", "DatabaseAddress", "DatabaseName", "DatabasePassword", "DatabaseUsername", "Descryption" },
                values: new object[,]
                {
                    { 1, "192.168.21.20", "fkf_test_db", "#sa2015!", "sa", "Citronex I - Symfonia ERP" },
                    { 2, "192.168.21.20", "fkf_goldfinch_test", "#sa2015!", "sa", "Citronex MOP - Symfonia ERP" }
                });

            migrationBuilder.InsertData(
                table: "Sources",
                columns: new[] { "SourceId", "Address", "Code", "DbName", "Description", "Password", "TargetId", "Type", "Username" },
                values: new object[] { 1, "192.168.42.70", "DP1", "sben", "Dyskont Paliwowy DP1", "almarwinnet", 1, "oracle_sben_dp", "sben" });

            migrationBuilder.InsertData(
                table: "Sources",
                columns: new[] { "SourceId", "Address", "Code", "DbName", "Description", "Password", "TargetId", "Type", "Username" },
                values: new object[] { 2, "192.168.45.70", "DP2", "sben", "Dyskont Paliwowy DP2", "almarwinnet", 1, "oracle_sben_dp", "sben" });

            migrationBuilder.InsertData(
                table: "Sources",
                columns: new[] { "SourceId", "Address", "Code", "DbName", "Description", "Password", "TargetId", "Type", "Username" },
                values: new object[] { 3, "I:\\DaneBP\\MOSTKI", "BPMOSTKI", "", "Stacja Paliw BP Mostki", "", 1, "bp_flat_file", "" });

            migrationBuilder.InsertData(
                table: "AccountingRecords",
                columns: new[] { "AccountingRecordId", "Account", "Credit", "Debit", "RecordOrder", "Sign", "SourceId" },
                values: new object[,]
                {
                    { 1, "199-9", "", "brutto", 1, "+", 1 },
                    { 2, "199-9", "netto", "", 2, "+", 1 },
                    { 3, "199-9", "vat", "", 3, "+", 1 },
                    { 4, "199-23", "", "brutto", 1, "+", 2 },
                    { 5, "199-23", "netto", "", 2, "+", 2 },
                    { 6, "199-23", "vat", "", 3, "+", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountingRecords_SourceId",
                table: "AccountingRecords",
                column: "SourceId");

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
                name: "IX_Documents_TaskTicketId",
                table: "Documents",
                column: "TaskTicketId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentUniqueInSource",
                table: "Documents",
                columns: new[] { "DocumentNumber", "SourceId" },
                unique: true,
                filter: "[DocumentNumber] IS NOT NULL AND [SourceId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentVats_DocumentId",
                table: "DocumentVats",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_Sources_TargetId",
                table: "Sources",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_DocSettingsShortcut",
                table: "TargetDocumentsSettings",
                column: "DocumentShortcut",
                unique: true,
                filter: "[DocumentShortcut] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_VatValueOnTargetDocument",
                table: "VatRegisters",
                columns: new[] { "TargetDocumentSettingsId", "VatValue" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountingRecords");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "DocumentVats");

            migrationBuilder.DropTable(
                name: "SourceTypes");

            migrationBuilder.DropTable(
                name: "VatRegisters");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "TargetDocumentsSettings");

            migrationBuilder.DropTable(
                name: "Contractors");

            migrationBuilder.DropTable(
                name: "TaskTickets");

            migrationBuilder.DropTable(
                name: "Sources");

            migrationBuilder.DropTable(
                name: "Targets");
        }
    }
}
