using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimruBackend.Migrations
{
    /// <inheritdoc />
    public partial class SeedPurposeData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 1,
                column: "Purpose",
                value: "Kebutuhan ruangan himpunan sastra mesin");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "Id",
                keyValue: 1,
                column: "Purpose",
                value: "");
        }
    }
}
