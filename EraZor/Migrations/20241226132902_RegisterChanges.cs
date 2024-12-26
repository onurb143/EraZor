using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EraZor.Migrations
{
    /// <inheritdoc />
    public partial class RegisterChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Disks",
                columns: table => new
                {
                    DiskID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Capacity = table.Column<int>(type: "integer", nullable: false),
                    Path = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    SerialNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Manufacturer = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Disks", x => x.DiskID);
                });

            migrationBuilder.CreateTable(
                name: "WipeMethods",
                columns: table => new
                {
                    WipeMethodID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    OverwritePass = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WipeMethods", x => x.WipeMethodID);
                });

            migrationBuilder.CreateTable(
                name: "WipeJobs",
                columns: table => new
                {
                    WipeJobId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    DiskId = table.Column<int>(type: "integer", nullable: false),
                    WipeMethodId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WipeJobs", x => x.WipeJobId);
                    table.ForeignKey(
                        name: "FK_WipeJobs_Disks_DiskId",
                        column: x => x.DiskId,
                        principalTable: "Disks",
                        principalColumn: "DiskID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WipeJobs_WipeMethods_WipeMethodId",
                        column: x => x.WipeMethodId,
                        principalTable: "WipeMethods",
                        principalColumn: "WipeMethodID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LogEntries",
                columns: table => new
                {
                    LogID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Message = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    WipeJobId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogEntries", x => x.LogID);
                    table.ForeignKey(
                        name: "FK_LogEntries_WipeJobs_WipeJobId",
                        column: x => x.WipeJobId,
                        principalTable: "WipeJobs",
                        principalColumn: "WipeJobId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "WipeMethods",
                columns: new[] { "WipeMethodID", "Description", "Name", "OverwritePass" },
                values: new object[,]
                {
                    { 1, "", "DoD 5220.22-M", 3 },
                    { 2, "", "NIST 800-88 Clear", 1 },
                    { 3, "", "NIST 800-88 Purge", 1 },
                    { 4, "", "Gutmann", 35 },
                    { 5, "", "Random Data", 1 },
                    { 6, "", "Write Zero", 1 },
                    { 7, "", "Write One", 1 },
                    { 8, "", "Schneider", 7 },
                    { 9, "", "Bruce Force", 10 },
                    { 10, "", "Quick Format", 1 },
                    { 11, "", "Full Format", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Disks_SerialNumber",
                table: "Disks",
                column: "SerialNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LogEntries_WipeJobId",
                table: "LogEntries",
                column: "WipeJobId");

            migrationBuilder.CreateIndex(
                name: "IX_WipeJobs_DiskId",
                table: "WipeJobs",
                column: "DiskId");

            migrationBuilder.CreateIndex(
                name: "IX_WipeJobs_WipeMethodId",
                table: "WipeJobs",
                column: "WipeMethodId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogEntries");

            migrationBuilder.DropTable(
                name: "WipeJobs");

            migrationBuilder.DropTable(
                name: "Disks");

            migrationBuilder.DropTable(
                name: "WipeMethods");
        }
    }
}
