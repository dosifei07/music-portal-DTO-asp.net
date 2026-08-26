using Microsoft.EntityFrameworkCore;
using MusicPortal.DataAccess.Models;
using System;
using System.Collections.Generic;

namespace MusicPortal.DataAccess.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<Song> Songs { get; set; } = null!;
        public DbSet<Genre> Genres { get; set; } = null!;
        public DbSet<Artist> Artists { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<Rating> Ratings { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.ConfigureWarnings(w =>
                w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(b =>
            {
                b.Property(u => u.Username).IsRequired().HasMaxLength(100);
                b.Property(u => u.Email).IsRequired().HasMaxLength(150);
                b.Property(u => u.PasswordHash).IsRequired();
                b.HasIndex(u => u.Email).IsUnique();
            });

            modelBuilder.Entity<User>()
                .HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserRoles",
                    j => j.HasOne<Role>().WithMany().HasForeignKey("RoleId").OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<User>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.Cascade)
                );

            modelBuilder.Entity<Role>()
                .Property(r => r.Name).IsRequired().HasMaxLength(50);

            modelBuilder.Entity<Genre>()
                .Property(g => g.Name).IsRequired().HasMaxLength(50);

            modelBuilder.Entity<Artist>(b =>
            {
                b.Property(a => a.Name).IsRequired().HasMaxLength(100);
                b.Property(a => a.Bio).HasMaxLength(1000);
            });

            modelBuilder.Entity<Artist>()
                .HasOne(a => a.User)
                .WithOne(u => u.ArtistProfile)
                .HasForeignKey<Artist>("UserId")
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);

            modelBuilder.Entity<Song>(b =>
            {
                b.Property(s => s.Title).IsRequired().HasMaxLength(150);
                b.Property(s => s.FilePath).IsRequired();

                b.HasOne(s => s.Artist)
                    .WithMany(a => a.Songs)
                    .HasForeignKey(s => s.ArtistId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Property(s => s.Rating)
                    .HasColumnType("float")
                    .HasDefaultValue(0);
            });

            modelBuilder.Entity<Song>()
                .HasMany(s => s.Genres)
                .WithMany(g => g.Songs)
                .UsingEntity<Dictionary<string, object>>(
                    "SongGenre",
                    j => j.HasOne<Genre>().WithMany().HasForeignKey("GenreId").OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<Song>().WithMany().HasForeignKey("SongId").OnDelete(DeleteBehavior.Cascade)
                );

            modelBuilder.Entity<Comment>(b =>
            {
                b.Property(c => c.Text).IsRequired().HasMaxLength(1000);

                b.HasOne(c => c.Song).WithMany(s => s.Comments)
                    .HasForeignKey("SongId").OnDelete(DeleteBehavior.Cascade);

                b.HasOne(c => c.User).WithMany(u => u.Comments)
                    .HasForeignKey("UserId").OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Rating>(b =>
            {
                b.HasIndex("UserId", "SongId").IsUnique();

                b.HasOne(r => r.Song).WithMany(s => s.Ratings)
                    .HasForeignKey("SongId").OnDelete(DeleteBehavior.Cascade);

                b.HasOne(r => r.User).WithMany(u => u.Ratings)
                    .HasForeignKey("UserId").OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "User" },
                new Role { Id = 3, Name = "Artist" },
                new Role { Id = 4, Name = "Pending" }
            );

            modelBuilder.Entity<Genre>().HasData(
                new Genre { Id = 1, Name = "Музыкаre" },
                new Genre { Id = 2, Name = "Музыкант" },
                new Genre { Id = 3, Name = "Рок" },
                new Genre { Id = 4, Name = "movie-asp.net-mvc" },
                new Genre { Id = 5, Name = "string" },
                new Genre { Id = 6, Name = "string" },
                new Genre { Id = 7, Name = "string" }
            );

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "admin@gmail.com", Email = "admin@gmail.com", PasswordHash = "AQAAAAIAAYagAAAAEPFXVCoAGGUMBXoztUUqpA7fF/Q++PPdjr6YS1gOcF0Zsy71lwAQ7ZKG2ODK6c3SFA==", IsApproved = true },
                new User { Id = 2, Username = "Ufrseg", Email = "Ufrseg@gmail.com", PasswordHash = "AQAAAAIAAYagAAAAEHOI/ldB8Glyb742DsfFgtuivnInziJ0de0fIi1ragjEQkuxEEMkTQnngilAfzcZvQ==" },
                new User { Id = 3, Username = "ab21506A", Email = "ab21506a@gmail.com", PasswordHash = "AQAAAAIAAYagAAAAEOVd22mqA1OXfJgD9IlpIOQWgPvkWkZskdEsjAfJJ00pgGAPqJa32H7d75EOGMswtg==" },
                new User { Id = 4, Username = "string", Email = "string@gmail.com", PasswordHash = "AQAAAAIAAYagAAAAEKgCQ0N4wM7QKgYXrh88fYIqMy/XmogWpgnv8T/Z8w/fgztnlK8WkHyhumSvNcNl6A==" }
            );

            modelBuilder.Entity("UserRoles").HasData(
                new { RoleId = 1, UserId = 1 },
                new { RoleId = 2, UserId = 1 },
                new { RoleId = 3, UserId = 1 },
                new { RoleId = 2, UserId = 2 },
                new { RoleId = 2, UserId = 3 }
            );

            modelBuilder.Entity<Artist>().HasData(
                new { Id = 1, Name = "Музыкант", Bio = "Поёт", UserId = (int?)null },
                new { Id = 2, Name = "Reas", Bio = "RE", UserId = (int?)2 },
                new { Id = 3, Name = "Музыка", Bio = "tf", UserId = (int?)null },
                new { Id = 4, Name = "Sdf", Bio = "fDs", UserId = (int?)null }
            );

            modelBuilder.Entity<Song>().HasData(
                new Song { Id = 2, Title = "Песня 1", FilePath = "/uploads/songs/6ce779ef-29f4-4ecd-9780-e989e6b4f6e7.mp3", Rating = 5.0, ArtistId = 1 },
                new Song { Id = 3, Title = "Песня 2", FilePath = "/uploads/songs/7e0cedd2-3379-4da6-a7ff-76223ebed9b1.mp3", Rating = 4.0, ArtistId = 1 },
                new Song { Id = 4, Title = "Песня 3", FilePath = "/uploads/songs/6ce779ef-29f4-4ecd-9780-e989e6b4f6e7.mp3", Rating = 0.0, ArtistId = 2 },
                new Song { Id = 5, Title = "Песня 4", FilePath = "/uploads/songs/df34ccce-986f-4191-886d-59c848541446.mp3", Rating = 0.0, ArtistId = 1 }
            );

            modelBuilder.Entity("SongGenre").HasData(
                new { SongId = 2, GenreId = 2 },
                new { SongId = 2, GenreId = 3 },
                new { SongId = 3, GenreId = 4 },
                new { SongId = 3, GenreId = 5 },
                new { SongId = 4, GenreId = 5 },
                new { SongId = 5, GenreId = 5 }
            );

            modelBuilder.Entity<Comment>().HasData(
                new { Id = 2, Text = "Ку", CreatedAt = DateTime.Parse("2026-07-20 12:48:09.1939717"), SongId = 3, UserId = 1 },
                new { Id = 4, Text = "gt", CreatedAt = DateTime.Parse("2026-08-25 23:16:32.9780934"), SongId = 2, UserId = 3 }
            );

            modelBuilder.Entity<Rating>().HasData(
                new { Id = 2, Value = 5, SongId = 2, UserId = 1 },
                new { Id = 3, Value = 4, SongId = 3, UserId = 1 }
            );
        }
    }
}