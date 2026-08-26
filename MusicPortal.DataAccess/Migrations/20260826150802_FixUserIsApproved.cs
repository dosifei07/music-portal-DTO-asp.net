using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicPortal.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixUserIsApproved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 2,
                column: "UploadDate",
                value: new DateTime(2026, 8, 26, 15, 8, 1, 848, DateTimeKind.Utc).AddTicks(407));

            migrationBuilder.UpdateData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 3,
                column: "UploadDate",
                value: new DateTime(2026, 8, 26, 15, 8, 1, 848, DateTimeKind.Utc).AddTicks(1100));

            migrationBuilder.UpdateData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 4,
                column: "UploadDate",
                value: new DateTime(2026, 8, 26, 15, 8, 1, 848, DateTimeKind.Utc).AddTicks(1103));

            migrationBuilder.UpdateData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 5,
                column: "UploadDate",
                value: new DateTime(2026, 8, 26, 15, 8, 1, 848, DateTimeKind.Utc).AddTicks(1105));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "IsApproved" },
                values: new object[] { new DateTime(2026, 8, 26, 15, 8, 1, 847, DateTimeKind.Utc).AddTicks(6847), true });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 26, 15, 8, 1, 847, DateTimeKind.Utc).AddTicks(7432));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 26, 15, 8, 1, 847, DateTimeKind.Utc).AddTicks(7435));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 26, 15, 8, 1, 847, DateTimeKind.Utc).AddTicks(7436));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 2,
                column: "UploadDate",
                value: new DateTime(2026, 8, 26, 14, 59, 4, 199, DateTimeKind.Utc).AddTicks(7563));

            migrationBuilder.UpdateData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 3,
                column: "UploadDate",
                value: new DateTime(2026, 8, 26, 14, 59, 4, 199, DateTimeKind.Utc).AddTicks(8815));

            migrationBuilder.UpdateData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 4,
                column: "UploadDate",
                value: new DateTime(2026, 8, 26, 14, 59, 4, 199, DateTimeKind.Utc).AddTicks(8820));

            migrationBuilder.UpdateData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 5,
                column: "UploadDate",
                value: new DateTime(2026, 8, 26, 14, 59, 4, 199, DateTimeKind.Utc).AddTicks(8822));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "IsApproved" },
                values: new object[] { new DateTime(2026, 8, 26, 14, 59, 4, 199, DateTimeKind.Utc).AddTicks(1507), false });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 26, 14, 59, 4, 199, DateTimeKind.Utc).AddTicks(2015));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 26, 14, 59, 4, 199, DateTimeKind.Utc).AddTicks(2018));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 26, 14, 59, 4, 199, DateTimeKind.Utc).AddTicks(2020));
        }
    }
}
