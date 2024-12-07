using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EraZor.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWipeMethodRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WipeJobs_WipeMethods_MethodId",
                table: "WipeJobs");

            migrationBuilder.DropColumn(
                name: "JobID",
                table: "LogEntries");

            migrationBuilder.RenameColumn(
                name: "MethodId",
                table: "WipeJobs",
                newName: "WipeMethodId");

            migrationBuilder.RenameIndex(
                name: "IX_WipeJobs_MethodId",
                table: "WipeJobs",
                newName: "IX_WipeJobs_WipeMethodId");

            migrationBuilder.RenameColumn(
                name: "DiskId",
                table: "Disks",
                newName: "DiskID");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Disks",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Disks",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Manufacturer",
                table: "Disks",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "Disks",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SerialNumber",
                table: "Disks",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WipeJobs_WipeMethods_WipeMethodId",
                table: "WipeJobs",
                column: "WipeMethodId",
                principalTable: "WipeMethods",
                principalColumn: "MethodID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WipeJobs_WipeMethods_WipeMethodId",
                table: "WipeJobs");

            migrationBuilder.DropColumn(
                name: "Manufacturer",
                table: "Disks");

            migrationBuilder.DropColumn(
                name: "Path",
                table: "Disks");

            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "Disks");

            migrationBuilder.RenameColumn(
                name: "WipeMethodId",
                table: "WipeJobs",
                newName: "MethodId");

            migrationBuilder.RenameIndex(
                name: "IX_WipeJobs_WipeMethodId",
                table: "WipeJobs",
                newName: "IX_WipeJobs_MethodId");

            migrationBuilder.RenameColumn(
                name: "DiskID",
                table: "Disks",
                newName: "DiskId");

            migrationBuilder.AddColumn<int>(
                name: "JobID",
                table: "LogEntries",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Disks",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Disks",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_WipeJobs_WipeMethods_MethodId",
                table: "WipeJobs",
                column: "MethodId",
                principalTable: "WipeMethods",
                principalColumn: "MethodID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
