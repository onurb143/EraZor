using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EraZor.Migrations
{
    /// <inheritdoc />
    public partial class RemoveMethodIdFromWipeJobs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WipeJobs_Disks_DiskId",
                table: "WipeJobs");

            migrationBuilder.DropForeignKey(
                name: "FK_WipeJobs_WipeMethods_WipeMethodId",
                table: "WipeJobs");

            migrationBuilder.DropColumn(
                name: "MethodId",
                table: "WipeJobs");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Disks");

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

            migrationBuilder.AddForeignKey(
                name: "FK_WipeJobs_Disks_DiskId",
                table: "WipeJobs",
                column: "DiskId",
                principalTable: "Disks",
                principalColumn: "DiskID");

            migrationBuilder.AddForeignKey(
                name: "FK_WipeJobs_WipeMethods_WipeMethodId",
                table: "WipeJobs",
                column: "WipeMethodId",
                principalTable: "WipeMethods",
                principalColumn: "WipeMethodID");
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

            migrationBuilder.AddColumn<int>(
                name: "MethodId",
                table: "WipeJobs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Disks",
                type: "text",
                nullable: false,
                defaultValue: "");

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
    }
}
