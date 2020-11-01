using Microsoft.EntityFrameworkCore.Migrations;

namespace FvpWebApp.Data.Migrations
{
    public partial class SourceDefsData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.InsertData(
                table: "TargetDocumentsSettings",
                columns: new[] { "TargetDocumentSettingsId", "DocumentShortcut", "SourceId", "VatRegisterId" },
                values: new object[,]
                {
                    { 1, "FDZG", 1, 1 },
                    { 2, "FDD2", 2, 1 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AccountingRecords",
                keyColumn: "AccountingRecordId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AccountingRecords",
                keyColumn: "AccountingRecordId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AccountingRecords",
                keyColumn: "AccountingRecordId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AccountingRecords",
                keyColumn: "AccountingRecordId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AccountingRecords",
                keyColumn: "AccountingRecordId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AccountingRecords",
                keyColumn: "AccountingRecordId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "TargetDocumentsSettings",
                keyColumn: "TargetDocumentSettingsId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TargetDocumentsSettings",
                keyColumn: "TargetDocumentSettingsId",
                keyValue: 2);
        }
    }
}
