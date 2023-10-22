using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Base.Database.Migrations
{
    public partial class Settingsadded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Settings");

            migrationBuilder.CreateTable(
                name: "Masters",
                schema: "Settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SettingLabel = table.Column<string>(type: "text", nullable: false),
                    SettingValue = table.Column<string>(type: "text", nullable: true),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Masters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Masters_Masters_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "Settings",
                        principalTable: "Masters",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Masters_ParentId",
                schema: "Settings",
                table: "Masters",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Masters_SettingLabel",
                schema: "Settings",
                table: "Masters",
                column: "SettingLabel");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Masters",
                schema: "Settings");
        }
    }
}
