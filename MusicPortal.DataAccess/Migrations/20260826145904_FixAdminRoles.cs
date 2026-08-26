using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicPortal.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FixAdminRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { 2, 1 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 26, 14, 59, 4, 199, DateTimeKind.Utc).AddTicks(1507));

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.UpdateData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 2,
                column: "UploadDate",
                value: new DateTime(2026, 8, 26, 14, 54, 32, 135, DateTimeKind.Utc).AddTicks(7631));

            migrationBuilder.UpdateData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 3,
                column: "UploadDate",
                value: new DateTime(2026, 8, 26, 14, 54, 32, 135, DateTimeKind.Utc).AddTicks(8323));

            migrationBuilder.UpdateData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 4,
                column: "UploadDate",
                value: new DateTime(2026, 8, 26, 14, 54, 32, 135, DateTimeKind.Utc).AddTicks(8325));

            migrationBuilder.UpdateData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 5,
                column: "UploadDate",
                value: new DateTime(2026, 8, 26, 14, 54, 32, 135, DateTimeKind.Utc).AddTicks(8327));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 26, 14, 54, 32, 135, DateTimeKind.Utc).AddTicks(4385));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 26, 14, 54, 32, 135, DateTimeKind.Utc).AddTicks(4999));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 26, 14, 54, 32, 135, DateTimeKind.Utc).AddTicks(5001));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 8, 26, 14, 54, 32, 135, DateTimeKind.Utc).AddTicks(5015));
        }
    }
}
