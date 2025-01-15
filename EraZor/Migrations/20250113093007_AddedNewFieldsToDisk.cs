using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EraZor.Migrations
{
    /// <inheritdoc />
    public partial class AddedNewFieldsToDisk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "WipeMethods",
                keyColumn: "WipeMethodID",
                keyValue: 1,
                columns: new[] { "Description", "OverwritePass" },
                values: new object[] { "Sikker metode, der udføres på hardware-niveau via SSD-controlleren. Ideel til SSD'er og NVMe. Ikke ISO-certificeret.", 1 });

            migrationBuilder.UpdateData(
                table: "WipeMethods",
                keyColumn: "WipeMethodID",
                keyValue: 2,
                column: "Description",
                value: "Overskriver med nulværdier i ét gennemløb. Velegnet til HDD'er, mindre egnet til SSD'er pga. wear leveling. Ikke ISO-certificeret.");

            migrationBuilder.UpdateData(
                table: "WipeMethods",
                keyColumn: "WipeMethodID",
                keyValue: 3,
                column: "Description",
                value: "Overskriver med tilfældige data i ét gennemløb. Velegnet til HDD'er, mindre egnet til SSD'er. Ikke ISO-certificeret.");

            migrationBuilder.UpdateData(
                table: "WipeMethods",
                keyColumn: "WipeMethodID",
                keyValue: 4,
                column: "Description",
                value: "Avanceret metode med 35 gennemløb designet til ældre HDD'er. Ikke egnet til moderne HDD'er, SSD'er eller NVMe. Ikke ISO-certificeret.");

            migrationBuilder.UpdateData(
                table: "WipeMethods",
                keyColumn: "WipeMethodID",
                keyValue: 5,
                column: "Description",
                value: "Overskriver med tilfældige data i 3 gennemløb. Velegnet til HDD'er, mindre egnet til SSD'er. Ikke ISO-certificeret.");

            migrationBuilder.UpdateData(
                table: "WipeMethods",
                keyColumn: "WipeMethodID",
                keyValue: 6,
                column: "Description",
                value: "Skriver nulværdier i ét gennemløb. God til HDD'er, mindre effektiv på SSD'er. Ikke ISO-certificeret.");

            migrationBuilder.UpdateData(
                table: "WipeMethods",
                keyColumn: "WipeMethodID",
                keyValue: 7,
                column: "Description",
                value: "Metode med 7 gennemløb, som er sikker og velegnet til HDD'er. Overkill for SSD'er og NVMe. Ikke ISO-certificeret.");

            migrationBuilder.UpdateData(
                table: "WipeMethods",
                keyColumn: "WipeMethodID",
                keyValue: 8,
                column: "Description",
                value: "Standardiseret metode med 3 gennemløb. Velegnet til HDD'er. Ikke egnet til SSD'er eller NVMe. Ikke ISO-certificeret.");

            migrationBuilder.UpdateData(
                table: "WipeMethods",
                keyColumn: "WipeMethodID",
                keyValue: 9,
                column: "Description",
                value: "Ekstremt sikker metode med 35 gennemløb, designet til ældre HDD'er. Ikke egnet til SSD'er eller NVMe. Ikke ISO-certificeret.");

            migrationBuilder.UpdateData(
                table: "WipeMethods",
                keyColumn: "WipeMethodID",
                keyValue: 10,
                column: "Description",
                value: "Hurtig metode med ét gennemløb af nulværdier. Velegnet til HDD'er, men mindre effektiv for SSD'er pga. wear leveling. Ikke ISO-certificeret.");

            migrationBuilder.UpdateData(
                table: "WipeMethods",
                keyColumn: "WipeMethodID",
                keyValue: 11,
                column: "Description",
                value: "DoD-standard med 4 gennemløb. Velegnet til HDD'er, mindre relevant for SSD'er. Ikke ISO-certificeret.");

            migrationBuilder.UpdateData(
                table: "WipeMethods",
                keyColumn: "WipeMethodID",
                keyValue: 12,
                column: "Description",
                value: "ISO-standardiseret metode med ét gennemløb af nulværdier. Ideel til SSD'er, NVMe og HDD'er. ISO-certificeret.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "WipeMethods",
                keyColumn: "WipeMethodID",
                keyValue: 1,
                columns: new[] { "Description", "OverwritePass" },
                values: new object[] { "Standard DoD-sletning med 3 gennemløb. Ikke ISO-certificeret.", 3 });

            migrationBuilder.UpdateData(
                table: "WipeMethods",
                keyColumn: "WipeMethodID",
                keyValue: 2,
                column: "Description",
                value: "Skriver nulværdier i ét gennemløb. Ikke ISO-certificeret.");

            migrationBuilder.UpdateData(
                table: "WipeMethods",
                keyColumn: "WipeMethodID",
                keyValue: 3,
                column: "Description",
                value: "Skriver tilfældige data i ét gennemløb. Ikke ISO-certificeret.");

            migrationBuilder.UpdateData(
                table: "WipeMethods",
                keyColumn: "WipeMethodID",
                keyValue: 4,
                column: "Description",
                value: "Meget sikker metode med 35 gennemløb. Ikke ISO-certificeret.");

            migrationBuilder.UpdateData(
                table: "WipeMethods",
                keyColumn: "WipeMethodID",
                keyValue: 5,
                column: "Description",
                value: "Skriver tilfældige data i 3 gennemløb. Ikke ISO-certificeret.");

            migrationBuilder.UpdateData(
                table: "WipeMethods",
                keyColumn: "WipeMethodID",
                keyValue: 6,
                column: "Description",
                value: "Skriver nulværdier i ét gennemløb. Ikke ISO-certificeret.");

            migrationBuilder.UpdateData(
                table: "WipeMethods",
                keyColumn: "WipeMethodID",
                keyValue: 7,
                column: "Description",
                value: "Sikker metode med 7 gennemløb. Ikke ISO-certificeret.");

            migrationBuilder.UpdateData(
                table: "WipeMethods",
                keyColumn: "WipeMethodID",
                keyValue: 8,
                column: "Description",
                value: "Sletning med 3 gennemløb efter britisk standard. Ikke ISO-certificeret.");

            migrationBuilder.UpdateData(
                table: "WipeMethods",
                keyColumn: "WipeMethodID",
                keyValue: 9,
                column: "Description",
                value: "Ekstremt sikker metode med 35 gennemløb. Ikke ISO-certificeret.");

            migrationBuilder.UpdateData(
                table: "WipeMethods",
                keyColumn: "WipeMethodID",
                keyValue: 10,
                column: "Description",
                value: "Hurtig sletning med ét gennemløb af nulværdier. Ikke ISO-certificeret.");

            migrationBuilder.UpdateData(
                table: "WipeMethods",
                keyColumn: "WipeMethodID",
                keyValue: 11,
                column: "Description",
                value: "Forbedret DoD-sletning med 4 gennemløb. Ikke ISO-certificeret.");

            migrationBuilder.UpdateData(
                table: "WipeMethods",
                keyColumn: "WipeMethodID",
                keyValue: 12,
                column: "Description",
                value: "ISO-standard med ét gennemløb af nulværdier. ISO-certificeret.");
        }
    }
}
