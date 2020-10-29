using Microsoft.EntityFrameworkCore.Migrations;

namespace FvpWebApp.Data.Migrations
{
    public partial class AccountingRecord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "IX_AccountingRecords_SourceId",
                table: "AccountingRecords",
                column: "SourceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountingRecords");

            migrationBuilder.DropTable(
                name: "TargetDocumentsSettings");
        }
    }
}
