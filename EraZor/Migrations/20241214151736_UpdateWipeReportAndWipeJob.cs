using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EraZor.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWipeReportAndWipeJob : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WipeReport_WipeJobs_WipeJobId1",
                table: "WipeReport");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WipeReport",
                table: "WipeReport");

            migrationBuilder.DropIndex(
                name: "IX_WipeReport_WipeJobId1",
                table: "WipeReport");

            migrationBuilder.RenameColumn(
                name: "WipeJobId1",
                table: "WipeReport",
                newName: "ReportId");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "WipeReport",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "DiskType",
                table: "WipeReport",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "WipeJobId",
                table: "WipeReport",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "ReportId",
                table: "WipeReport",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "ReportId",
                table: "WipeReport",
                newName: "WipeJobId1");

            migrationBuilder.AlterColumn<int>(
                name: "WipeJobId",
                table: "WipeReport",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "WipeReport",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DiskType",
                table: "WipeReport",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "WipeJobId1",
                table: "WipeReport",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WipeReport",
                table: "WipeReport",
                column: "WipeJobId");

            migrationBuilder.CreateIndex(
                name: "IX_WipeReport_WipeJobId1",
                table: "WipeReport",
                column: "WipeJobId1");

            migrationBuilder.AddForeignKey(
                name: "FK_WipeReport_WipeJobs_WipeJobId1",
                table: "WipeReport",
                column: "WipeJobId1",
                principalTable: "WipeJobs",
                principalColumn: "WipeJobId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
