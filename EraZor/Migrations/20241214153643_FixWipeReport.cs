using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EraZor.Migrations
{
    /// <inheritdoc />
    public partial class FixWipeReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WipeReport_WipeJobs_WipeJobId",
                table: "WipeReport");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WipeReport",
                table: "WipeReport");

            migrationBuilder.DropIndex(
                name: "IX_WipeReport_WipeJobId",
                table: "WipeReport");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "WipeReport");

            migrationBuilder.RenameTable(
                name: "WipeReport",
                newName: "WipeReports");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WipeReports",
                table: "WipeReports",
                column: "WipeJobId");

            migrationBuilder.AddForeignKey(
                name: "FK_WipeReports_WipeJobs_WipeJobId",
                table: "WipeReports",
                column: "WipeJobId",
                principalTable: "WipeJobs",
                principalColumn: "WipeJobId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WipeReports_WipeJobs_WipeJobId",
                table: "WipeReports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WipeReports",
                table: "WipeReports");

            migrationBuilder.RenameTable(
                name: "WipeReports",
                newName: "WipeReport");

            migrationBuilder.AddColumn<int>(
                name: "ReportId",
                table: "WipeReport",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WipeReport",
                table: "WipeReport",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_WipeReport_WipeJobId",
                table: "WipeReport",
                column: "WipeJobId");

            migrationBuilder.AddForeignKey(
                name: "FK_WipeReport_WipeJobs_WipeJobId",
                table: "WipeReport",
                column: "WipeJobId",
                principalTable: "WipeJobs",
                principalColumn: "WipeJobId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
