using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EraZor.Migrations
{
    /// <inheritdoc />
    public partial class InitialSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SupportedDiskType",
                table: "WipeMethods");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SupportedDiskType",
                table: "WipeMethods",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
