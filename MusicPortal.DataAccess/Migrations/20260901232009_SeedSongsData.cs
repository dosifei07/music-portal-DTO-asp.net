using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicPortal.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedSongsData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 2,
                column: "UploadDate",
                value: new DateTime(2026, 9, 1, 23, 20, 8, 827, DateTimeKind.Utc).AddTicks(9324));

            migrationBuilder.UpdateData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 3,
                column: "UploadDate",
                value: new DateTime(2026, 9, 1, 23, 20, 8, 828, DateTimeKind.Utc).AddTicks(118));

            migrationBuilder.UpdateData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 4,
                column: "UploadDate",
                value: new DateTime(2026, 9, 1, 23, 20, 8, 828, DateTimeKind.Utc).AddTicks(122));

            migrationBuilder.UpdateData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 5,
                column: "UploadDate",
                value: new DateTime(2026, 9, 1, 23, 20, 8, 828, DateTimeKind.Utc).AddTicks(124));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEPFXVCoAGGUMBXoztUUqpA7fF/Q++PPdjr6YS1gOcF0Zsy71lwAQ7ZKG2ODK6c3SFA==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 2,
                column: "UploadDate",
                value: new DateTime(2026, 8, 31, 14, 31, 44, 595, DateTimeKind.Utc).AddTicks(8414));

            migrationBuilder.UpdateData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 3,
                column: "UploadDate",
                value: new DateTime(2026, 8, 31, 14, 31, 44, 595, DateTimeKind.Utc).AddTicks(9099));

            migrationBuilder.UpdateData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 4,
                column: "UploadDate",
                value: new DateTime(2026, 8, 31, 14, 31, 44, 595, DateTimeKind.Utc).AddTicks(9102));

            migrationBuilder.UpdateData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 5,
                column: "UploadDate",
                value: new DateTime(2026, 8, 31, 14, 31, 44, 595, DateTimeKind.Utc).AddTicks(9103));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "ВАШ_СГЕНЕРИРОВАННЫЙ_ХЭШ");
        }
    }
}
