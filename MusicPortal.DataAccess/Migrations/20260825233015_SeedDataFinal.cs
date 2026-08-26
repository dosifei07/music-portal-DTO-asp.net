using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MusicPortal.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Artists",
                columns: new[] { "Id", "Bio", "Name", "UserId" },
                values: new object[,]
                {
                    { 1, "Поёт", "Музыкант", null },
                    { 3, "tf", "Музыка", null },
                    { 4, "fDs", "Sdf", null }
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Музыкаre" },
                    { 2, "Музыкант" },
                    { 3, "Рок" },
                    { 4, "movie-asp.net-mvc" },
                    { 5, "string" },
                    { 6, "string" },
                    { 7, "string" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "IsApproved", "PasswordHash", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 8, 25, 23, 30, 15, 432, DateTimeKind.Utc).AddTicks(8094), "admin@gmail.com", false, "AQAAAAIAAYagAAAAEPFXVCoAGGUMBXoztUUqpA7fF/Q++PPdjr6YS1gOcF0Zsy71lwAQ7ZKG2ODK6c3SFA==", "admin@gmail.com" },
                    { 2, new DateTime(2026, 8, 25, 23, 30, 15, 432, DateTimeKind.Utc).AddTicks(8668), "Ufrseg@gmail.com", false, "AQAAAAIAAYagAAAAEHOI/ldB8Glyb742DsfFgtuivnInziJ0de0fIi1ragjEQkuxEEMkTQnngilAfzcZvQ==", "Ufrseg" },
                    { 3, new DateTime(2026, 8, 25, 23, 30, 15, 432, DateTimeKind.Utc).AddTicks(8672), "Ufrseg4@gmail.com", false, "AQAAAAIAAYagAAAAEOVd22mqA1OXfJgD9IlpIOQWgPvkWkZskdEsjAfJJ00pgGAPqJa32H7d75EOGMswtg==", "ab21506A" },
                    { 4, new DateTime(2026, 8, 25, 23, 30, 15, 432, DateTimeKind.Utc).AddTicks(8673), "string", false, "AQAAAAIAAYagAAAAEKgCQ0N4wM7QKgYXrh88fYIqMy/XmogWpgnv8T/Z8w/fgztnlK8WkHyhumSvNcNl6A==", "string" }
                });

            migrationBuilder.InsertData(
                table: "Artists",
                columns: new[] { "Id", "Bio", "Name", "UserId" },
                values: new object[] { 2, "RE", "Reas", 2 });

            migrationBuilder.InsertData(
                table: "Songs",
                columns: new[] { "Id", "ArtistId", "FilePath", "PlayCount", "Rating", "Title", "UploadDate" },
                values: new object[,]
                {
                    { 2, 1, "/uploads/songs/6ce779ef-29f4-4ecd-9780-e989e6b4f6e7.mp3", 0, 5.0, "Песня 1", new DateTime(2026, 8, 25, 23, 30, 15, 433, DateTimeKind.Utc).AddTicks(2071) },
                    { 3, 1, "/uploads/songs/7e0cedd2-3379-4da6-a7ff-76223ebed9b1.mp3", 0, 4.0, "Песня 2", new DateTime(2026, 8, 25, 23, 30, 15, 433, DateTimeKind.Utc).AddTicks(2861) }
                });

            migrationBuilder.InsertData(
                table: "Songs",
                columns: new[] { "Id", "ArtistId", "FilePath", "PlayCount", "Title", "UploadDate" },
                values: new object[] { 5, 1, "/uploads/songs/df34ccce-986f-4191-886d-59c848541446.mp3", 0, "Песня 4", new DateTime(2026, 8, 25, 23, 30, 15, 433, DateTimeKind.Utc).AddTicks(2865) });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 2, 3 },
                    { 3, 1 }
                });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "CreatedAt", "SongId", "Text", "UserId" },
                values: new object[,]
                {
                    { 2, new DateTime(2026, 7, 20, 12, 48, 9, 193, DateTimeKind.Unspecified).AddTicks(9717), 3, "Ку", 1 },
                    { 4, new DateTime(2026, 8, 25, 23, 16, 32, 978, DateTimeKind.Unspecified).AddTicks(934), 2, "gt", 3 }
                });

            migrationBuilder.InsertData(
                table: "Ratings",
                columns: new[] { "Id", "SongId", "UserId", "Value" },
                values: new object[,]
                {
                    { 2, 2, 1, 5 },
                    { 3, 3, 1, 4 }
                });

            migrationBuilder.InsertData(
                table: "SongGenre",
                columns: new[] { "GenreId", "SongId" },
                values: new object[,]
                {
                    { 2, 2 },
                    { 3, 2 },
                    { 4, 3 },
                    { 5, 3 },
                    { 5, 5 }
                });

            migrationBuilder.InsertData(
                table: "Songs",
                columns: new[] { "Id", "ArtistId", "FilePath", "PlayCount", "Title", "UploadDate" },
                values: new object[] { 4, 2, "C:\\Users\\dosif\\Downloads\\music-portal-DTO-asp.net-main\\music-portal-DTO-asp.net-main\\music-portal-asp.net\\wwwroot\\uploads\\songs\\6ce779ef-29f4-4ecd-9780-e989e6b4f6e7.mp3", 0, "Песня 3", new DateTime(2026, 8, 25, 23, 30, 15, 433, DateTimeKind.Utc).AddTicks(2864) });

            migrationBuilder.InsertData(
                table: "SongGenre",
                columns: new[] { "GenreId", "SongId" },
                values: new object[] { 5, 4 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Artists",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Artists",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Ratings",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Ratings",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "SongGenre",
                keyColumns: new[] { "GenreId", "SongId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "SongGenre",
                keyColumns: new[] { "GenreId", "SongId" },
                keyValues: new object[] { 3, 2 });

            migrationBuilder.DeleteData(
                table: "SongGenre",
                keyColumns: new[] { "GenreId", "SongId" },
                keyValues: new object[] { 4, 3 });

            migrationBuilder.DeleteData(
                table: "SongGenre",
                keyColumns: new[] { "GenreId", "SongId" },
                keyValues: new object[] { 5, 3 });

            migrationBuilder.DeleteData(
                table: "SongGenre",
                keyColumns: new[] { "GenreId", "SongId" },
                keyValues: new object[] { 5, 4 });

            migrationBuilder.DeleteData(
                table: "SongGenre",
                keyColumns: new[] { "GenreId", "SongId" },
                keyValues: new object[] { 5, 5 });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Songs",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Artists",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Artists",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
