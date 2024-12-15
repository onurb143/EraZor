using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EraZor.Migrations
{
    /// <inheritdoc />
    public partial class FixWipeJobId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WipeJobs_Disks_DiskId",
                table: "WipeJobs");

            migrationBuilder.DropForeignKey(
                name: "FK_WipeJobs_IdentityUser_UserId",
                table: "WipeJobs");

            migrationBuilder.DropForeignKey(
                name: "FK_WipeJobs_WipeMethods_WipeMethodId",
                table: "WipeJobs");

            migrationBuilder.DropIndex(
                name: "IX_WipeJobs_UserId",
                table: "WipeJobs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IdentityUser",
                table: "IdentityUser");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "WipeJobs");

            migrationBuilder.RenameTable(
                name: "IdentityUser",
                newName: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "WipeMethodId",
                table: "WipeJobs",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DiskId",
                table: "WipeJobs",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Disks",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "SerialNumber",
                table: "Disks",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Path",
                table: "Disks",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Manufacturer",
                table: "Disks",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "WipeReport",
                columns: table => new
                {
                    WipeJobId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    DiskType = table.Column<string>(type: "text", nullable: false),
                    Capacity = table.Column<int>(type: "integer", nullable: false),
                    SerialNumber = table.Column<string>(type: "text", nullable: false),
                    Manufacturer = table.Column<string>(type: "text", nullable: false),
                    WipeMethodName = table.Column<string>(type: "text", nullable: false),
                    OverwritePasses = table.Column<int>(type: "integer", nullable: false),
                    WipeJobId1 = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WipeReport", x => x.WipeJobId);
                    table.ForeignKey(
                        name: "FK_WipeReport_WipeJobs_WipeJobId1",
                        column: x => x.WipeJobId1,
                        principalTable: "WipeJobs",
                        principalColumn: "WipeJobId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WipeReport_WipeJobId1",
                table: "WipeReport",
                column: "WipeJobId1");

            migrationBuilder.AddForeignKey(
                name: "FK_WipeJobs_Disks_DiskId",
                table: "WipeJobs",
                column: "DiskId",
                principalTable: "Disks",
                principalColumn: "DiskID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WipeJobs_WipeMethods_WipeMethodId",
                table: "WipeJobs",
                column: "WipeMethodId",
                principalTable: "WipeMethods",
                principalColumn: "WipeMethodID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WipeJobs_Disks_DiskId",
                table: "WipeJobs");

            migrationBuilder.DropForeignKey(
                name: "FK_WipeJobs_WipeMethods_WipeMethodId",
                table: "WipeJobs");

            migrationBuilder.DropTable(
                name: "WipeReport");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "IdentityUser");

            migrationBuilder.AlterColumn<int>(
                name: "WipeMethodId",
                table: "WipeJobs",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "DiskId",
                table: "WipeJobs",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "WipeJobs",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Disks",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "SerialNumber",
                table: "Disks",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Path",
                table: "Disks",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Manufacturer",
                table: "Disks",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_IdentityUser",
                table: "IdentityUser",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_WipeJobs_UserId",
                table: "WipeJobs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WipeJobs_Disks_DiskId",
                table: "WipeJobs",
                column: "DiskId",
                principalTable: "Disks",
                principalColumn: "DiskID");

            migrationBuilder.AddForeignKey(
                name: "FK_WipeJobs_IdentityUser_UserId",
                table: "WipeJobs",
                column: "UserId",
                principalTable: "IdentityUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WipeJobs_WipeMethods_WipeMethodId",
                table: "WipeJobs",
                column: "WipeMethodId",
                principalTable: "WipeMethods",
                principalColumn: "WipeMethodID");
        }
    }
}
