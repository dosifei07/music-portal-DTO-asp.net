using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MusicPortal.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Artists_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.RoleId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Songs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UploadDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PlayCount = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<double>(type: "float", nullable: false, defaultValue: 0.0),
                    ArtistId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Songs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Songs_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SongId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Songs_SongId",
                        column: x => x.SongId,
                        principalTable: "Songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<int>(type: "int", nullable: false),
                    SongId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ratings_Songs_SongId",
                        column: x => x.SongId,
                        principalTable: "Songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ratings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SongGenre",
                columns: table => new
                {
                    GenreId = table.Column<int>(type: "int", nullable: false),
                    SongId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SongGenre", x => new { x.GenreId, x.SongId });
                    table.ForeignKey(
                        name: "FK_SongGenre_Genres_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SongGenre_Songs_SongId",
                        column: x => x.SongId,
                        principalTable: "Songs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "User" },
                    { 3, "Artist" },
                    { 4, "Pending" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "IsApproved", "PasswordHash", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 8, 26, 14, 54, 32, 135, DateTimeKind.Utc).AddTicks(4385), "admin@gmail.com", false, "AQAAAAIAAYagAAAAEPFXVCoAGGUMBXoztUUqpA7fF/Q++PPdjr6YS1gOcF0Zsy71lwAQ7ZKG2ODK6c3SFA==", "admin@gmail.com" },
                    { 2, new DateTime(2026, 8, 26, 14, 54, 32, 135, DateTimeKind.Utc).AddTicks(4999), "Ufrseg@gmail.com", false, "AQAAAAIAAYagAAAAEHOI/ldB8Glyb742DsfFgtuivnInziJ0de0fIi1ragjEQkuxEEMkTQnngilAfzcZvQ==", "Ufrseg" },
                    { 3, new DateTime(2026, 8, 26, 14, 54, 32, 135, DateTimeKind.Utc).AddTicks(5001), "ab21506a@gmail.com", false, "AQAAAAIAAYagAAAAEOVd22mqA1OXfJgD9IlpIOQWgPvkWkZskdEsjAfJJ00pgGAPqJa32H7d75EOGMswtg==", "ab21506A" },
                    { 4, new DateTime(2026, 8, 26, 14, 54, 32, 135, DateTimeKind.Utc).AddTicks(5015), "string@gmail.com", false, "AQAAAAIAAYagAAAAEKgCQ0N4wM7QKgYXrh88fYIqMy/XmogWpgnv8T/Z8w/fgztnlK8WkHyhumSvNcNl6A==", "string" }
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
                    { 2, 1, "/uploads/songs/6ce779ef-29f4-4ecd-9780-e989e6b4f6e7.mp3", 0, 5.0, "Песня 1", new DateTime(2026, 8, 26, 14, 54, 32, 135, DateTimeKind.Utc).AddTicks(7631) },
                    { 3, 1, "/uploads/songs/7e0cedd2-3379-4da6-a7ff-76223ebed9b1.mp3", 0, 4.0, "Песня 2", new DateTime(2026, 8, 26, 14, 54, 32, 135, DateTimeKind.Utc).AddTicks(8323) }
                });

            migrationBuilder.InsertData(
                table: "Songs",
                columns: new[] { "Id", "ArtistId", "FilePath", "PlayCount", "Title", "UploadDate" },
                values: new object[] { 5, 1, "/uploads/songs/df34ccce-986f-4191-886d-59c848541446.mp3", 0, "Песня 4", new DateTime(2026, 8, 26, 14, 54, 32, 135, DateTimeKind.Utc).AddTicks(8327) });

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
                values: new object[] { 4, 2, "/uploads/songs/6ce779ef-29f4-4ecd-9780-e989e6b4f6e7.mp3", 0, "Песня 3", new DateTime(2026, 8, 26, 14, 54, 32, 135, DateTimeKind.Utc).AddTicks(8325) });

            migrationBuilder.InsertData(
                table: "SongGenre",
                columns: new[] { "GenreId", "SongId" },
                values: new object[] { 5, 4 });

            migrationBuilder.CreateIndex(
                name: "IX_Artists_UserId",
                table: "Artists",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_SongId",
                table: "Comments",
                column: "SongId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_SongId",
                table: "Ratings",
                column: "SongId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UserId_SongId",
                table: "Ratings",
                columns: new[] { "UserId", "SongId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SongGenre_SongId",
                table: "SongGenre",
                column: "SongId");

            migrationBuilder.CreateIndex(
                name: "IX_Songs_ArtistId",
                table: "Songs",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropTable(
                name: "SongGenre");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Songs");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Artists");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
