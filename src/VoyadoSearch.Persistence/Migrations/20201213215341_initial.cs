using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VoyadoSearch.Persistence.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "search");

            migrationBuilder.EnsureSchema(
                name: "cache");

            migrationBuilder.CreateTable(
                name: "History",
                schema: "search",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Term = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Updated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_History", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Search",
                schema: "cache",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EngineId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Term = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Hits = table.Column<long>(type: "bigint", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Updated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Search", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SearchCache_Created",
                schema: "cache",
                table: "Search",
                column: "Created");

            migrationBuilder.CreateIndex(
                name: "IX_SearchCache_EngineId",
                schema: "cache",
                table: "Search",
                column: "EngineId");

            migrationBuilder.CreateIndex(
                name: "IX_SearchCache_Term",
                schema: "cache",
                table: "Search",
                column: "Term",
                filter: "[Term] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "History",
                schema: "search");

            migrationBuilder.DropTable(
                name: "Search",
                schema: "cache");
        }
    }
}
