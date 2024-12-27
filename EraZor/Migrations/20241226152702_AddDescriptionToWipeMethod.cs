using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EraZor.Migrations
{
    /// <inheritdoc />
    public partial class AddDescriptionToWipeMethod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "WipeMethods",
                keyColumn: "WipeMethodID",
                keyValue: 1,
                column: "Description",
                value: "Standard DoD wiping method with 3 passes");

            migrationBuilder.UpdateData(
                table: "WipeMethods",
                keyColumn: "WipeMethodID",
                keyValue: 2,
                column: "Description",
                value: "NIST standard for clearing data with 1 pass");

            migrationBuilder.UpdateData(
                table: "WipeMethods",
                keyColumn: "WipeMethodID",
                keyValue: 3,
                column: "Description",
                value: "NIST standard for purging data with 1 pass");

            migrationBuilder.UpdateData(
                table: "WipeMethods",
                keyColumn: "WipeMethodID",
                keyValue: 4,
                column: "Description",
                value: "Highly secure method with 35 overwrite passes");

            migrationBuilder.UpdateData(
                table: "WipeMethods",
                keyColumn: "WipeMethodID",
                keyValue: 5,
                column: "Description",
                value: "Single pass of random data");

            migrationBuilder.UpdateData(
                table: "WipeMethods",
                keyColumn: "WipeMethodID",
                keyValue: 6,
                column: "Description",
                value: "Single pass of zeroes");

            migrationBuilder.UpdateData(
                table: "WipeMethods",
                keyColumn: "WipeMethodID",
                keyValue: 7,
                column: "Description",
                value: "Single pass of ones");

            migrationBuilder.UpdateData(
                table: "WipeMethods",
                keyColumn: "WipeMethodID",
                keyValue: 8,
                column: "Description",
                value: "Custom 7-pass wiping method");

            migrationBuilder.UpdateData(
                table: "WipeMethods",
                keyColumn: "WipeMethodID",
                keyValue: 9,
                column: "Description",
                value: "Secure 10-pass overwrite method");

            migrationBuilder.UpdateData(
                table: "WipeMethods",
                keyColumn: "WipeMethodID",
                keyValue: 10,
                column: "Description",
                value: "Fast format with 1 pass");

            migrationBuilder.UpdateData(
                table: "WipeMethods",
                keyColumn: "WipeMethodID",
                keyValue: 11,
                column: "Description",
                value: "Complete format with 1 pass");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "WipeMethods",
                keyColumn: "WipeMethodID",
                keyValue: 1,
                column: "Description",
                value: "");

            migrationBuilder.UpdateData(
                table: "WipeMethods",
                keyColumn: "WipeMethodID",
                keyValue: 2,
                column: "Description",
                value: "");

            migrationBuilder.UpdateData(
                table: "WipeMethods",
                keyColumn: "WipeMethodID",
                keyValue: 3,
                column: "Description",
                value: "");

            migrationBuilder.UpdateData(
                table: "WipeMethods",
                keyColumn: "WipeMethodID",
                keyValue: 4,
                column: "Description",
                value: "");

            migrationBuilder.UpdateData(
                table: "WipeMethods",
                keyColumn: "WipeMethodID",
                keyValue: 5,
                column: "Description",
                value: "");

            migrationBuilder.UpdateData(
                table: "WipeMethods",
                keyColumn: "WipeMethodID",
                keyValue: 6,
                column: "Description",
                value: "");

            migrationBuilder.UpdateData(
                table: "WipeMethods",
                keyColumn: "WipeMethodID",
                keyValue: 7,
                column: "Description",
                value: "");

            migrationBuilder.UpdateData(
                table: "WipeMethods",
                keyColumn: "WipeMethodID",
                keyValue: 8,
                column: "Description",
                value: "");

            migrationBuilder.UpdateData(
                table: "WipeMethods",
                keyColumn: "WipeMethodID",
                keyValue: 9,
                column: "Description",
                value: "");

            migrationBuilder.UpdateData(
                table: "WipeMethods",
                keyColumn: "WipeMethodID",
                keyValue: 10,
                column: "Description",
                value: "");

            migrationBuilder.UpdateData(
                table: "WipeMethods",
                keyColumn: "WipeMethodID",
                keyValue: 11,
                column: "Description",
                value: "");
        }
    }
}
