using Microsoft.EntityFrameworkCore.Migrations;

namespace FvpWebApp.Data.Migrations
{
    public partial class VatRegistersToVatCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DocumentShortcut",
                table: "TargetDocumentsSettings",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_DocSettingsShortcut",
                table: "TargetDocumentsSettings",
                column: "DocumentShortcut",
                unique: true,
                filter: "[DocumentShortcut] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_VatRegisters_TargetDocumentSettingsId",
                table: "VatRegisters",
                column: "TargetDocumentSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_VatValue",
                table: "VatRegisters",
                column: "VatValue",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VatRegisters");

            migrationBuilder.DropIndex(
                name: "IX_DocSettingsShortcut",
                table: "TargetDocumentsSettings");

            migrationBuilder.AlterColumn<string>(
                name: "DocumentShortcut",
                table: "TargetDocumentsSettings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
